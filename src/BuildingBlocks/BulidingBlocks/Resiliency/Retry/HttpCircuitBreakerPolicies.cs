using System;
using System.Net.Http;
using BuildingBlocks.Resiliency.Configs;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;

namespace BuildingBlocks.Resiliency
{
    public static class HttpCircuitBreakerPolicies
    {
        public static AsyncCircuitBreakerPolicy<HttpResponseMessage> GetHttpCircuitBreakerPolicy(ILogger logger,
            ICircuitBreakerPolicyConfig circuitBreakerPolicyConfig)
        {
            return HttpPolicyBuilders.GetBaseBuilder()
                .CircuitBreakerAsync(circuitBreakerPolicyConfig.RetryCount + 1,
                    TimeSpan.FromSeconds(circuitBreakerPolicyConfig.BreakDuration),
                    (result, breakDuration) =>
                    {
                        OnHttpBreak(result, breakDuration, circuitBreakerPolicyConfig.RetryCount, logger);
                    },
                    () => { OnHttpReset(logger); });
        }

        public static void OnHttpBreak(DelegateResult<HttpResponseMessage> result, TimeSpan breakDuration,
            int retryCount, ILogger logger)
        {
            logger.LogWarning("Service shutdown during {breakDuration} after {DefaultRetryCount} failed retries.",
                breakDuration, retryCount);
            throw new BrokenCircuitException("Service inoperative. Please try again later");
        }

        public static void OnHttpReset(ILogger logger)
        {
            logger.LogInformation("Service restarted.");
        }
    }
}