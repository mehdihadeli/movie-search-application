namespace BuildingBlocks.Resiliency.Configs
{
    public interface ITimeoutPolicy
    {
        public int TimeOutDuration { get; set; }
    }
}