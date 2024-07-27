using BuildingBlocks.Domain;
using MovieSearch.Application.TvShows.Dtos;

namespace MovieSearch.Application.TvShows.Features.SearchTVShow;

public class SearchTVShowQueryResult
{
    public ListResultModel<TVShowInfoDto> TVShowList { get; init; }
}
