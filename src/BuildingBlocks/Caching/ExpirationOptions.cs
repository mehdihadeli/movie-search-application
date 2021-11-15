using System;

namespace BuildingBlocks.Caching
{
    public class ExpirationOptions
    {
        public ExpirationOptions(DateTimeOffset absoluteExpiration)
        {
            AbsoluteExpiration = absoluteExpiration;
        }

        public DateTimeOffset AbsoluteExpiration { get; }
    }
}