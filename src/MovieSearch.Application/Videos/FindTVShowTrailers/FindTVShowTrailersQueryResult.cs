using MovieSearch.Application.Videos.Dtos;
using MovieSearch.Core.Generals;

namespace MovieSearch.Application.Videos.FindTVShowTrailers
{
    public class FindTVShowTrailersQueryResult
    {
        public VideoListResultModel<VideoDto> VideoList { get; init; }
    }
}