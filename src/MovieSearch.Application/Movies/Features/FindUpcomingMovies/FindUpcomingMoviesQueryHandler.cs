using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using MovieSearch.Application.Movies.Dtos;
using MovieSearch.Application.Services.Clients;

namespace MovieSearch.Application.Movies.Features.FindUpcomingMovies;

public class FindUpcomingMoviesQueryHandler : IRequestHandler<FindUpcomingMoviesQuery, FindUpcomingMoviesQueryResult>
{
    private readonly IMapper _mapper;
    private readonly IMovieDbServiceClient _movieDbServiceClient;

    public FindUpcomingMoviesQueryHandler(IMovieDbServiceClient movieDbServiceClient, IMapper mapper)
    {
        _movieDbServiceClient = movieDbServiceClient;
        _mapper = mapper;
    }

    public async Task<FindUpcomingMoviesQueryResult> Handle(
        FindUpcomingMoviesQuery query,
        CancellationToken cancellationToken
    )
    {
        Guard.Against.Null(query, nameof(FindUpcomingMoviesQuery));

        var movies = await _movieDbServiceClient.GetUpComingMoviesAsync(query.Page, cancellationToken);

        var result = movies.Map(x => _mapper.Map<MovieInfoDto>(x));

        return new FindUpcomingMoviesQueryResult { MovieList = result };
    }
}
