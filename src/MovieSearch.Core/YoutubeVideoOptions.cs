using Google.Apis.YouTube.v3;

namespace MovieSearch.Core
{
    public class YoutubeVideoOptions
    {
        public string ApiKey { get; set; }
        public string SearchPart { get; set; }
        public string SearchType { get; set; }
        public SearchResource.ListRequest.OrderEnum Order { get; set; }
    }
}