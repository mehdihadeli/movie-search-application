using MovieSearch.Application.Videos.Dtos;
using MovieSearch.Core.Generals;

namespace MovieSearch.Application.Videos.FindVideos
{
    public class FindVideosQueryResult
    {
        public VideoListResultModel<VideoDto> VideoList { get; init; }
    }
}