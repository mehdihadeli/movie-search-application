using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using MovieSearch.Application.Services.Clients;
using MovieSearch.Application.TvShows.Dtos;

namespace MovieSearch.Application.TvShows.Features.SearchTVShow;

public class SearchTVShowQueryHandler : IRequestHandler<SearchTVShowQuery, SearchTVShowQueryResult>
{
    private readonly IMapper _mapper;
    private readonly IMovieDbServiceClient _movieDbServiceClient;

    public SearchTVShowQueryHandler(IMovieDbServiceClient movieDbServiceClient, IMapper mapper)
    {
        _movieDbServiceClient = movieDbServiceClient;
        _mapper = mapper;
    }

    public async Task<SearchTVShowQueryResult> Handle(SearchTVShowQuery query, CancellationToken cancellationToken)
    {
        Guard.Against.Null(query, nameof(SearchTVShowQuery));

        var tvShows = await _movieDbServiceClient.SearchTvShowAsync(
            query.SearchKeywords,
            query.Page,
            query.IncludeAdult,
            query.FirstAirDateYear,
            cancellationToken
        );

        var result = tvShows.Map(x => _mapper.Map<TVShowInfoDto>(x));

        return new SearchTVShowQueryResult { TVShowList = result };
    }
}
