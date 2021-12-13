using System.Reflection;
using Ben.Diagnostics;
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

namespace MovieSearch.Api
{
    public class Startup
    {
        private const string ApiCorsPolicy = "api";
        
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
            services.AddOptions<AppOptions>().Bind(Configuration.GetSection(nameof(AppOptions))))
                .ValidateDataAnnotations();

            services.AddOptions<TMDBOptions>().Bind(Configuration.GetSection(nameof(TMDBOptions)))
                .ValidateDataAnnotations();

            services.AddOptions<YoutubeVideoOptions>().Bind(Configuration.GetSection(nameof(YoutubeVideoOptions)))
                .ValidateDataAnnotations();

            services.AddOptions<PolicyConfig>().Bind(Configuration.GetSection(nameof(PolicyConfig)))
                .ValidateDataAnnotations();

            services.AddControllers(options =>
                options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer())));

            services.AddHttpContextAccessor();

            services.AddCors(options =>
            {
                options.AddPolicy(ApiCorsPolicy, policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
            });

            services.AddCustomHttpClients(Configuration);
            services.AddCustomVersioning();
            //services.AddCustomHealthCheck(healthBuilder => { });
            services.AddCustomSwagger(Configuration, Assembly.GetExecutingAssembly());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddCustomApiKeyAuthentication();

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
            //https://codeopinion.com/detecting-sync-over-async-code-in-asp-net-core/
            app.UseBlockingDetection();

            app.UseInfrastructure(env);

            app.UseCors(ApiCorsPolicy);

            app.UseRouting();

            //app.UseCustomHealthCheck();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", context => context.Response.WriteAsync("Movie Search Api!"));
            });
        }
    }
}
