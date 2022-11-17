using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BuildingBlocks.Security.ApiKey;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BuildingBlocks.Swagger;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomSwagger(
        this IServiceCollection services,
        IConfiguration configuration,
        Assembly assembly)
    {
        //https://dotnetthoughts.net/openapi-support-for-aspnetcore-minimal-webapi/
        //https://jaliyaudagedara.blogspot.com/2021/07/net-6-preview-6-introducing-openapi.html
        services.AddEndpointsApiExplorer();

        services.AddOptions<SwaggerOptions>().Bind(configuration.GetSection(nameof(SwaggerOptions)))
            .ValidateDataAnnotations();

        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

        services.AddSwaggerGen(
            options =>
            {
                options.OperationFilter<SwaggerDefaultValues>();
                var xmlFile = XmlCommentsFilePath(assembly);
                if (File.Exists(xmlFile))
                    options.IncludeXmlComments(xmlFile);

                var bearerScheme = new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n
                              Enter 'Bearer' [space] and then your token in the text input below.
                              \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                };

                var apiKeyScheme = new OpenApiSecurityScheme
                {
                    Description = "Api key needed to access the endpoints. X-Api-Key: My_API_Key",
                    In = ParameterLocation.Header,
                    Name = ApiKeyConstants.HeaderName,
                    Type = SecuritySchemeType.ApiKey,
                    Reference = new OpenApiReference
                        {Type = ReferenceType.SecurityScheme, Id = ApiKeyConstants.HeaderName}
                };

                options.AddSecurityDefinition("Bearer", bearerScheme);
                options.AddSecurityDefinition(ApiKeyConstants.HeaderName, apiKeyScheme);

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        bearerScheme,
                        new List<string>()
                    },
                    {
                        apiKeyScheme,
                        new string[] { }
                    }
                });

                options.EnableAnnotations();
            });

        return services;

        static string XmlCommentsFilePath(Assembly assembly)
        {
            var basePath = PlatformServices.Default.Application.ApplicationBasePath;
            var fileName = assembly.GetName().Name + ".xml";
            return Path.Combine(basePath, fileName);
        }
    }

    public static IApplicationBuilder UseCustomSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(
            options =>
            {
                var descriptions = app.DescribeApiVersions();

                // build a swagger endpoint for each discovered API version
                foreach (var description in descriptions)
                {
                    var url = $"/swagger/{description.GroupName}/swagger.json";
                    var name = description.GroupName.ToUpperInvariant();
                    options.SwaggerEndpoint(url, name);
                }
            });

        return app;
    }
}