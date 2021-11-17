using BuildingBlocks.Resiliency.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Resiliency
{
    public static class HttpClientBuilderExtensions
    {
        public static IHttpClientBuilder AddCustomPolicyHandlers(this IHttpClientBuilder httpClientBuilder,
            IConfiguration configuration, string policySectionName)
        {
            var policyConfig = new PolicyConfig();
            configuration.Bind(policySectionName, policyConfig);

            var circuitBreakerPolicyConfig = (ICircuitBreakerPolicyConfig)policyConfig;
            var retryPolicyConfig = (IRetryPolicyConfig)policyConfig;

            return httpClientBuilder.AddRetryPolicyHandler(retryPolicyConfig)
                .AddCircuitBreakerHandler(circuitBreakerPolicyConfig);
        }

        public static IHttpClientBuilder AddRetryPolicyHandler(this IHttpClientBuilder httpClientBuilder,
            IRetryPolicyConfig retryPolicyConfig)
        {
            //https://stackoverflow.com/questions/53604295/logging-polly-wait-and-retry-policy-asp-net-core-2-1
            return httpClientBuilder.AddPolicyHandler((sp, _) =>
            {
                var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
                var retryLogger = loggerFactory.CreateLogger("PollyHttpRetryPoliciesLogger");

                return HttpRetryPolicies.GetHttpRetryPolicy(retryLogger, retryPolicyConfig);
            });
        }

        public static IHttpClientBuilder AddCircuitBreakerHandler(this IHttpClientBuilder httpClientBuilder,
            ICircuitBreakerPolicyConfig circuitBreakerPolicyConfig)
        {
            return httpClientBuilder.AddPolicyHandler((sp, _) =>
            {
                var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
                var circuitBreakerLogger = loggerFactory.CreateLogger("PollyHttpCircuitBreakerPoliciesLogger");

                return HttpCircuitBreakerPolicies.GetHttpCircuitBreakerPolicy(circuitBreakerLogger,
                    circuitBreakerPolicyConfig);
            });
        }
    }
}