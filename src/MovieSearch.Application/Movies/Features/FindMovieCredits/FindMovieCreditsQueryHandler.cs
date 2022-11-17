using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using MovieSearch.Application.Movies.Dtos;
using MovieSearch.Application.Movies.Exceptions;
using MovieSearch.Application.Services.Clients;

namespace MovieSearch.Application.Movies.Features.FindMovieCredits;

public class FindMovieCreditsQueryHandler : IRequestHandler<FindMovieCreditsQuery, FindMovieCreditsQueryResult>
{
    private readonly IMapper _mapper;
    private readonly IMovieDbServiceClient _movieDbServiceClient;

    public FindMovieCreditsQueryHandler(IMovieDbServiceClient movieDbServiceClient, IMapper mapper)
    {
        _movieDbServiceClient = movieDbServiceClient;
        _mapper = mapper;
    }

    public async Task<FindMovieCreditsQueryResult> Handle(FindMovieCreditsQuery query,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(query, nameof(FindMovieCreditsQuery));

        var movieCredit = await _movieDbServiceClient.GetMovieCreditsAsync(query.MovieId, cancellationToken);

        if (movieCredit is null)
            throw new MovieCreditsNotFoundException(query.MovieId);

        var result = _mapper.Map<MovieCreditDto>(movieCredit);

        return new FindMovieCreditsQueryResult {MovieCredit = result};
    }
}