using BuildingBlocks.Domain;

namespace MovieSearch.Application.Videos.FindVideos
{
    public class FindVideosQuery : IQuery<FindVideosQueryResult>
    {
        public string MovieName { get; init; }
        public string PageToken { get; init; } = "";
    }
}