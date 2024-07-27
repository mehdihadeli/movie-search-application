namespace BuildingBlocks.Resiliency.Configs;

public interface IRetryPolicyConfig
{
    int RetryCount { get; set; }
}
