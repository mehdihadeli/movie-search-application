using System.Net.Http;
using Polly;
using Polly.Extensions.Http;

namespace BuildingBlocks.Resiliency
{
    public static class HttpPolicyBuilders
    {
        public static PolicyBuilder<HttpResponseMessage> GetBaseBuilder()
        {
            return HttpPolicyExtensions.HandleTransientHttpError();
        }
    }
}