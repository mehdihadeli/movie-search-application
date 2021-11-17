using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using BuildingBlocks.Resiliency.Configs;
using BuildingBlocks.Swagger;
using BuildingBlocks.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MovieSearch.Application;
using MovieSearch.Core;
using MovieSearch.Infrastructure;
using MovieSearch.Infrastructure.Services.Clients;
using Polly;

namespace MovieSearch.Api
{
    public class Startup
    {
        private const string AppOptionsSectionName = "AppOptions";
        private const string TMDBOptionsSectionName = "TMDBOptions";
        private const string YoutubeVideoOptionsSectionName = "YoutubeVideoOptions";
        private const string PolicyConfigSectionName = "PolicyConfig";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public AppOptions AppOptions { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            AppOptions = Configuration.GetSection(AppOptionsSectionName).Get<AppOptions>();
            services.AddOptions<AppOptions>().Bind(Configuration.GetSection(AppOptionsSectionName))
                .ValidateDataAnnotations();

            services.AddOptions<TMDBOptions>().Bind(Configuration.GetSection(TMDBOptionsSectionName))
                .ValidateDataAnnotations();

            services.AddOptions<YoutubeVideoOptions>().Bind(Configuration.GetSection(YoutubeVideoOptionsSectionName))
                .ValidateDataAnnotations();

            services.AddOptions<PolicyConfig>().Bind(Configuration.GetSection(PolicyConfigSectionName))
                .ValidateDataAnnotations();


            services.AddControllers(options =>
                options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer())));

            services.AddHttpContextAccessor();

            services.AddCors(options =>
            {
                options.AddPolicy("api", policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
            });

            services.AddCustomHttpClients(Configuration);
            services.AddCustomVersioning();
            services.AddCustomHealthCheck(healthBuilder => { });
            services.AddCustomSwagger(Assembly.GetExecutingAssembly());

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddApplication();
            services.AddInfrastructure(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                var provider = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();
                app.UseCustomSwagger(provider);
            }

            app.UseHttpsRedirection();

            app.UseInfrastructure(env);

            app.UseCors("api");

            app.UseRouting();

            app.UseCustomHealthCheck();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", context => context.Response.WriteAsync("Movie Search Api!"));
            });
        }
    }
}