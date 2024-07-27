using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using MovieSearch.Application.Movies.Dtos;
using MovieSearch.Application.Services.Clients;

namespace MovieSearch.Application.Movies.Features.SearchMovieByTitle;

public class SearchMovieByTitleQueryHandler : IRequestHandler<SearchMovieByTitleQuery, SearchMovieByTitleQueryResult>
{
    private readonly IMapper _mapper;
    private readonly IMovieDbServiceClient _movieDbServiceClient;

    public SearchMovieByTitleQueryHandler(IMovieDbServiceClient movieDbServiceClient, IMapper mapper)
    {
        _movieDbServiceClient = movieDbServiceClient;
        _mapper = mapper;
    }

    public async Task<SearchMovieByTitleQueryResult> Handle(
        SearchMovieByTitleQuery query,
        CancellationToken cancellationToken
    )
    {
        Guard.Against.Null(query, nameof(SearchMovieByTitleQuery));

        var movies = await _movieDbServiceClient.SearchMovieAsync(
            query.SearchKeywords,
            query.Page,
            cancellationToken: cancellationToken
        );

        var result = movies.Map(x => _mapper.Map<MovieInfoDto>(x));

        return new SearchMovieByTitleQueryResult { MovieList = result };
    }
}
