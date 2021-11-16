using System;
using System.Collections.Generic;
using System.IO;
using BuildingBlocks.IntegrationTests.Mock;
using BuildingBlocks.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Xunit.Abstractions;
using BuildingBlocks.Web;

namespace BuildingBlocks.Test.Factories
{
    public class CustomApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint>
        where TEntryPoint : class
    {
        public IConfiguration Configuration => Services.GetRequiredService<IConfiguration>();
        public ITestOutputHelper OutputHelper { get; set; }
        public IEnumerable<IDataSeeder> DataSeeders { get; set; }
        public Action<IServiceCollection> TestRegistrationServices { get; set; }

        public CustomApplicationFactory() : this(null)
        {
        }

        public CustomApplicationFactory(Action<IServiceCollection> testRegistrationServices = null)
        {
            TestRegistrationServices = testRegistrationServices ?? (collection => { });
        }

        //use this if we use IHost and generic host
        protected override IHostBuilder CreateHostBuilder()
        {
            var builder = base.CreateHostBuilder();

            builder = builder.UseSerilog((_, _, configuration) =>
            {
                if (OutputHelper is not null)
                    configuration.WriteTo.Xunit(OutputHelper);

                configuration.MinimumLevel.Is(LogEventLevel.Information)
                    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                    .WriteTo.Console();
            });

            return builder;
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseContentRoot(Directory.GetCurrentDirectory());
            return base.CreateHost(builder);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // //we could read env from our test launch setting or we can set it directly here
            builder.UseEnvironment("test");

            //The test app's builder.ConfigureTestServices callback is executed after the app's Startup.ConfigureServices code is executed.
            builder.ConfigureTestServices((services) =>
            {
                services.ReplaceScoped<IDataSeeder, NullDataSeeder>();
                TestRegistrationServices?.Invoke(services);
            });

            //The test app's builder.ConfigureServices callback is executed before the SUT's Startup.ConfigureServices code.
            builder.ConfigureServices(services =>
            {
                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var seeders = scope.ServiceProvider.GetServices<IDataSeeder>();

                foreach (var seeder in seeders)
                {
                    seeder.SeedAllAsync().GetAwaiter().GetResult();
                }
            });

            builder.UseDefaultServiceProvider((env, c) =>
            {
                // Handling Captive Dependency Problem
                // https://ankitvijay.net/2020/03/17/net-core-and-di-beware-of-captive-dependency/
                // https://blog.ploeh.dk/2014/06/02/captive-dependency/
                if (env.HostingEnvironment.IsEnvironment("test") || env.HostingEnvironment.IsDevelopment())
                    c.ValidateScopes = true;
            });
        }
    }
}