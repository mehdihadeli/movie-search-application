using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Filters;

namespace BuildingBlocks.Logging
{
    public static class Extensions
    {
        private const string LoggerSectionName = "Logging";

        public static IHostBuilder UseCustomSerilog(this IHostBuilder builder,
            Action<LoggerConfiguration> extraConfigure = null,
            string loggerSectionName = LoggerSectionName)
        {
            return builder.UseSerilog((context, serviceProvider, loggerConfiguration) =>
            {
                var httpContext = serviceProvider.GetService<IHttpContextAccessor>();
                loggerConfiguration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(serviceProvider)
                    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                    .Enrich.FromLogContext();

                var loggerOptions = context.Configuration.GetSection(loggerSectionName).Get<LoggerOptions>();
                if (loggerOptions is { })
                    MapOptions(loggerOptions, loggerConfiguration, context);

                extraConfigure?.Invoke(loggerConfiguration);
            });
        }

        private static void MapOptions(LoggerOptions loggerOptions,
            LoggerConfiguration loggerConfiguration, HostBuilderContext hostBuilderContext)
        {
            var level = GetLogEventLevel(loggerOptions.Level);

            loggerConfiguration
                .MinimumLevel.Is(level)
                .Enrich.WithProperty("Environment", hostBuilderContext.HostingEnvironment.EnvironmentName);


            if (hostBuilderContext.HostingEnvironment.IsDevelopment())
            {
                loggerConfiguration.WriteTo.Console();
            }
            else
            {
                if (loggerOptions.UseElasticSearch)
                    loggerConfiguration.WriteTo.Elasticsearch(loggerOptions.ElasticSearchLoggingOptions?.Url);
                if (loggerOptions.UseSeq)
                    loggerConfiguration.WriteTo.Seq(Environment.GetEnvironmentVariable("SEQ_URL") ??
                                                    loggerOptions.SeqOptions.Url);
                loggerConfiguration.WriteTo.Console();
            }

            foreach (var (key, value) in loggerOptions.Tags ?? new Dictionary<string, object>())
                loggerConfiguration.Enrich.WithProperty(key, value);

            foreach (var (key, value) in loggerOptions.MinimumLevelOverrides ?? new Dictionary<string, string>())
            {
                var logLevel = GetLogEventLevel(value);
                loggerConfiguration.MinimumLevel.Override(key, logLevel);
            }

            loggerOptions.ExcludePaths?.ToList().ForEach(p => loggerConfiguration.Filter
                .ByExcluding(Matching.WithProperty<string>("RequestPath", n => n.EndsWith(p))));

            loggerOptions.ExcludeProperties?.ToList().ForEach(p => loggerConfiguration.Filter
                .ByExcluding(Matching.WithProperty(p)));
        }

        private static LogEventLevel GetLogEventLevel(string level)
        {
            return Enum.TryParse<LogEventLevel>(level, true, out var logLevel)
                ? logLevel
                : LogEventLevel.Information;
        }
    }
}