using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using MovieSearch.Application.Movies.Dtos;
using MovieSearch.Application.Services.Clients;

namespace MovieSearch.Application.Movies.Features.FindPopularMovies;

public class FindPopularMoviesQueryHandler : IRequestHandler<FindPopularMoviesQuery, FindPopularMoviesQueryResult>
{
    private readonly IMapper _mapper;
    private readonly IMovieDbServiceClient _movieDbServiceClient;

    public FindPopularMoviesQueryHandler(IMovieDbServiceClient movieDbServiceClient, IMapper mapper)
    {
        _movieDbServiceClient = movieDbServiceClient;
        _mapper = mapper;
    }

    public async Task<FindPopularMoviesQueryResult> Handle(FindPopularMoviesQuery query,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(query, nameof(FindPopularMoviesQuery));

        var movies = await _movieDbServiceClient.GetPopularMoviesAsync(query.Page, cancellationToken);

        var result = movies.Map(x => _mapper.Map<MovieInfoDto>(x));

        return new FindPopularMoviesQueryResult {MovieList = result};
    }
}