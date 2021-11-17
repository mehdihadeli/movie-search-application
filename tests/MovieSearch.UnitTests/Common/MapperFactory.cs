using AutoMapper;
using MovieSearch.Infrastructure;

namespace Orders.UnitTests.Common
{
    public static class MapperFactory
    {
        public static IMapper Create()
        {
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<InfrastructureMappings>();
            });

            return configurationProvider.CreateMapper();
        }
    }
}