using MovieSearch.Application.Videos.Dtos;
using MovieSearch.Core.Generals;

namespace MovieSearch.Application.Videos.FindTrailers
{
    public class FindTrailersQueryResult
    {
        public VideoListResultModel<VideoDto> VideoList { get; init; }
    }
}