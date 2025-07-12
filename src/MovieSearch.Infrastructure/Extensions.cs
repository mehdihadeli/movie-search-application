using System;
using System.Collections.Generic;
using System.Reflection;
using Ben.Diagnostics;
using BuildingBlocks.Caching;
using BuildingBlocks.Exception;
using BuildingBlocks.Logging;
using BuildingBlocks.Resiliency;
using BuildingBlocks.Resiliency.Configs;
using BuildingBlocks.Security.ApiKey;
using BuildingBlocks.Security.ApiKey.Authorization;
using BuildingBlocks.Swagger;
using BuildingBlocks.Validation;
using BuildingBlocks.Web;
using Google;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MovieSearch.Application;
using MovieSearch.Application.Services.Clients;
using MovieSearch.Core;
using MovieSearch.Infrastructure.Security;
using MovieSearch.Infrastructure.Services.Clients.MovieDb;
using MovieSearch.Infrastructure.Services.Clients.Video;
using Newtonsoft.Json;
using Serilog;

namespace MovieSearch.Infrastructure;

public static class Extensions
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        // https://www.talkingdotnet.com/disable-automatic-model-state-validation-in-asp-net-core-2-1/
        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        var appOptions = builder.Configuration.GetSection(nameof(AppOptions)).Get<AppOptions>();
        builder
            .Services.AddOptions<AppOptions>()
            .Bind(builder.Configuration.GetSection(nameof(AppOptions)))
            .ValidateDataAnnotations();

        builder
            .Services.AddOptions<TMDBOptions>()
            .Bind(builder.Configuration.GetSection(nameof(TMDBOptions)))
            .ValidateDataAnnotations();

        builder
            .Services.AddOptions<YoutubeVideoOptions>()
            .Bind(builder.Configuration.GetSection(nameof(YoutubeVideoOptions)))
            .ValidateDataAnnotations();

        builder
            .Services.AddOptions<PolicyConfig>()
            .Bind(builder.Configuration.GetSection(nameof(PolicyConfig)))
            .ValidateDataAnnotations();

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddCustomHttpClients(builder.Configuration);
        builder.Services.AddCustomVersioning();
        builder.Services.AddCustomHealthCheck(healthBuilder => { });
        builder.Services.AddCustomSwagger(builder.Configuration, Assembly.GetExecutingAssembly());
        builder.Services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());

        builder.Services.AddCustomApiKeyAuthentication();

        builder.Services.AddSingleton<IMovieDbServiceClient, TMDBServiceClient>();
        builder.Services.AddSingleton<IVideoServiceClient, YoutubeVideoServiceClient>();

        builder.Services.AddCustomValidators(typeof(ApplicationRoot).Assembly);

        builder
            .Services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(ApplicationRoot).Assembly))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(InvalidateCachingBehavior<,>));

        builder.Services.AddCachingRequestPolicies(new List<Assembly> { typeof(ApplicationRoot).Assembly });

        builder.Services.AddEasyCaching(options =>
        {
            options.UseInMemory(builder.Configuration, "mem");
        });

        AddCustomProblemDetails(builder);

        return builder;
    }

    public static void UseInfrastructure(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseProblemDetails();
        //https://codeopinion.com/detecting-sync-over-async-code-in-asp-net-core/
        app.UseBlockingDetection();
        app.UseSerilogRequestLogging();
        app.UseCustomHealthCheck();
    }

    private static void AddCustomProblemDetails(WebApplicationBuilder builder)
    {
        builder.Services.AddProblemDetails(x =>
        {
            // Control when an exception is included
            x.IncludeExceptionDetails = (ctx, _) =>
            {
                // Fetch services from HttpContext.RequestServices
                var env = ctx.RequestServices.GetRequiredService<IHostEnvironment>();
                return env.IsDevelopment() || env.IsStaging();
            };
            x.Map<AppException>(ex => new ProblemDetails
            {
                Title = "Application rule broken",
                Status = StatusCodes.Status409Conflict,
                Detail = ex.Message,
                Type = "https://somedomain/application-rule-validation-error"
            });
            // Exception will produce and returns from our FluentValidation RequestValidationBehavior
            x.Map<ValidationException>(ex => new ProblemDetails
            {
                Title = "input validation rules broken",
                Status = StatusCodes.Status400BadRequest,
                Detail = JsonConvert.SerializeObject(ex.ValidationResultModel.Errors),
                Type = "https://somedomain/input-validation-rules-error"
            });
            x.Map<BadRequestException>(ex => new ProblemDetails
            {
                Title = "bad request exception",
                Status = StatusCodes.Status400BadRequest,
                Detail = ex.Message,
                Type = "https://somedomain/bad-request-error"
            });
            x.Map<NotFoundException>(ex => new ProblemDetails
            {
                Title = "not found exception",
                Status = StatusCodes.Status404NotFound,
                Detail = ex.Message,
                Type = "https://somedomain/not-found-error"
            });
            x.Map<ApiException>(ex => new ProblemDetails
            {
                Title = "api server exception",
                Status = StatusCodes.Status500InternalServerError,
                Detail = ex.Message,
                Type = "https://somedomain/api-server-error"
            });
            x.MapToStatusCode<ArgumentNullException>(StatusCodes.Status400BadRequest);
            x.Map<GoogleApiException>(googleApiException =>
            {
                if (googleApiException.Error.Code == StatusCodes.Status403Forbidden)
                    return new ProblemDetails
                    {
                        Title = "youtube api forbidden exception",
                        Status = StatusCodes.Status403Forbidden,
                        Detail = googleApiException.Error.Message,
                        Type = "https://somedomain/forbiden"
                    };

                if (googleApiException.Error.Code == StatusCodes.Status400BadRequest)
                    return new ProblemDetails
                    {
                        Title = "youtube api bad request exception",
                        Status = StatusCodes.Status400BadRequest,
                        Detail = googleApiException.Error.Message,
                        Type = "https://somedomain/bad-request-error"
                    };

                return new ProblemDetails();
            });
        });
    }

    public static IServiceCollection AddCustomHttpClients(
        this IServiceCollection services,
        IConfiguration configuration,
        string movieDbSectionName = "TMDBOptions",
        string pollySectionName = "PolicyConfig"
    )
    {
        var movieDbOptions = configuration.GetSection(movieDbSectionName).Get<TMDBOptions>();

        services
            .AddHttpClient(
                nameof(TMDBServiceClient),
                config =>
                {
                    config.BaseAddress = new Uri(movieDbOptions.BaseApiAddress);
                    config.Timeout = new TimeSpan(0, 0, 30);
                    config.DefaultRequestHeaders.Clear();
                }
            )
            .AddCustomPolicyHandlers(configuration, pollySectionName);

        return services;
    }

    public static IServiceCollection AddCustomApiKeyAuthentication(this IServiceCollection services)
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = ApiKeyAuthenticationOptions.DefaultScheme;
                options.DefaultChallengeScheme = ApiKeyAuthenticationOptions.DefaultScheme;
            })
            .AddApiKeySupport(options => { });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(
                Policies.OnlyCustomers,
                policy => policy.Requirements.Add(new OnlyCustomersRequirement())
            );
            options.AddPolicy(Policies.OnlyAdmins, policy => policy.Requirements.Add(new OnlyAdminsRequirement()));
            options.AddPolicy(
                Policies.OnlyThirdParties,
                policy => policy.Requirements.Add(new OnlyThirdPartiesRequirement())
            );
        });

        services.AddSingleton<IAuthorizationHandler, OnlyCustomersAuthorizationHandler>();
        services.AddSingleton<IAuthorizationHandler, OnlyAdminsAuthorizationHandler>();
        services.AddSingleton<IAuthorizationHandler, OnlyThirdPartiesAuthorizationHandler>();

        services.AddSingleton<IGetApiKeyQuery, InMemoryGetApiKeyQuery>();

        return services;
    }
}
