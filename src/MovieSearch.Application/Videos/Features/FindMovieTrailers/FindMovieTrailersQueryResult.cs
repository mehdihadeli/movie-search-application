using MovieSearch.Application.Videos.Dtos;
using MovieSearch.Core.Generals;

namespace MovieSearch.Application.Videos.Features.FindMovieTrailers;

public class FindMovieTrailersQueryResult
{
    public VideoListResultModel<VideoDto> VideoList { get; init; }
}
