using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BuildingBlocks.Web
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCustomHealthCheck(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/healthz", new HealthCheckOptions
            {
                Predicate = _ => true,
                // ResponseWriter = async (context, report) =>
                // {
                //     context.Response.ContentType = "application/json";
                //
                //     var result = JsonConvert.SerializeObject(new
                //     {
                //         Status = report.Status.ToString(),
                //         HealthChecks = report.Entries.Select(x => new
                //         {
                //             Components = x.Key,
                //             Status = x.Value.Status.ToString(),
                //             Description = x.Value.Description
                //         }),
                //         HealthCheckDuration = report.TotalDuration
                //     });
                //
                //     await context.Response.WriteAsync(result);
                // },
                //https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                ResultStatusCodes =
                {
                    [HealthStatus.Healthy] = StatusCodes.Status200OK,
                    [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
                    [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
                },
            }).UseHealthChecksUI(setup =>
            {
                setup.ApiPath = "/healthcheck";
                setup.UIPath = "/healthcheck-ui";
            });

            return app;
        }
    }
}