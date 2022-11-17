using MovieSearch.Application.Videos.Dtos;
using MovieSearch.Core.Generals;

namespace MovieSearch.Application.Videos.Features.FindTrailers;

public class FindTrailersQueryResult
{
    public VideoListResultModel<VideoDto> VideoList { get; init; }
}