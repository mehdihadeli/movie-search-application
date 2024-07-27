namespace BuildingBlocks.Resiliency.Configs;

public interface ICircuitBreakerPolicyConfig
{
    int RetryCount { get; set; }
    int BreakDuration { get; set; }
}
