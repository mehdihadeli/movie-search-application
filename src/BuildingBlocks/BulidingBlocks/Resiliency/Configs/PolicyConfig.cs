namespace BuildingBlocks.Resiliency.Configs
{
    public class PolicyConfig : ICircuitBreakerPolicyConfig, IRetryPolicyConfig,ITimeoutPolicy
    {
        public int RetryCount { get; set; }
        public int BreakDuration { get; set; }
        public int TimeOutDuration { get; set; }
    }
}