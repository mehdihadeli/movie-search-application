using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.SpectreConsole;

namespace BuildingBlocks.Logging;

public static class Extensions
{
    public static WebApplicationBuilder AddCustomSerilog(
        this WebApplicationBuilder builder,
        Action<LoggerConfiguration>? extraConfigure = null
    )
    {
        builder.Host.UseSerilog(
            (context, serviceProvider, loggerConfiguration) =>
            {
                var loggerOptions = context.Configuration.GetSection(nameof(LoggerOptions)).Get<LoggerOptions>();

                extraConfigure?.Invoke(loggerConfiguration);

                loggerConfiguration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(serviceProvider)
                    .Enrich.WithExceptionDetails()
                    .Enrich.WithCorrelationId()
                    .Enrich.FromLogContext();

                var level = Enum.TryParse<LogEventLevel>(loggerOptions?.Level, true, out var logLevel)
                    ? logLevel
                    : LogEventLevel.Information;

                // https://andrewlock.net/using-serilog-aspnetcore-in-asp-net-core-3-reducing-log-verbosity/
                loggerConfiguration
                    .MinimumLevel.Is(level)
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                    .MinimumLevel
                    // Filter out ASP.NET Core infrastructure logs that are Information and below
                    .Override("Microsoft.AspNetCore", LogEventLevel.Warning);

                if (context.HostingEnvironment.IsDevelopment())
                {
                    loggerConfiguration.WriteTo.Async(writeTo =>
                        writeTo.SpectreConsole(
                            loggerOptions?.LogTemplate
                                ?? "{Timestamp:HH:mm:ss} [{Level:u4}] {Message:lj}{NewLine}{Exception}",
                            level
                        )
                    );
                }
                else
                {
                    if (!string.IsNullOrEmpty(loggerOptions?.ElasticSearchUrl))
                        loggerConfiguration.WriteTo.Async(writeTo =>
                            writeTo.Elasticsearch(loggerOptions.ElasticSearchUrl)
                        );

                    if (!string.IsNullOrEmpty(loggerOptions?.SeqUrl))
                        loggerConfiguration.WriteTo.Async(writeTo => writeTo.Seq(loggerOptions.SeqUrl));
                }

                if (!string.IsNullOrEmpty(loggerOptions?.LogPath))
                    loggerConfiguration.WriteTo.File(
                        loggerOptions.LogPath,
                        outputTemplate: loggerOptions.LogTemplate
                            ?? "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {Level} - {Message:lj}{NewLine}{Exception}",
                        rollingInterval: RollingInterval.Day,
                        rollOnFileSizeLimit: true
                    );
            }
        );

        return builder;
    }
}
