using BuildingBlocks.Domain;
using MovieSearch.Application.TvShows.Dtos;

namespace MovieSearch.Application.TvShows.Features.SearchTVShowByTitle
{
    public class SearchTVShowByTitleQueryResult
    {
        public ListResultModel<TVShowInfoDto> TVShowList { get; init; }
    }
}