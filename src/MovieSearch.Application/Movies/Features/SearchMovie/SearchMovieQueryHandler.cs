using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using MovieSearch.Application.Movies.Dtos;
using MovieSearch.Application.Services.Clients;

namespace MovieSearch.Application.Movies.Features.SearchMovie;

public class SearchMovieQueryHandler : IRequestHandler<SearchMovieQuery, SearchMovieQueryResult>
{
    private readonly IMapper _mapper;
    private readonly IMovieDbServiceClient _movieDbServiceClient;

    public SearchMovieQueryHandler(IMovieDbServiceClient movieDbServiceClient, IMapper mapper)
    {
        _movieDbServiceClient = movieDbServiceClient;
        _mapper = mapper;
    }

    public async Task<SearchMovieQueryResult> Handle(SearchMovieQuery query, CancellationToken cancellationToken)
    {
        Guard.Against.Null(query, nameof(SearchMovieQuery));

        var movies = await _movieDbServiceClient.SearchMovieAsync(
            query.SearchKeywords,
            query.Page,
            query.IncludeAdult,
            query.Year,
            query.PrimaryReleaseYear,
            cancellationToken
        );

        var result = movies.Map(x => _mapper.Map<MovieInfoDto>(x));

        return new SearchMovieQueryResult { MovieList = result };
    }
}
