using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using MovieSearch.Application.Movies.Dtos;
using MovieSearch.Application.Movies.Exceptions;
using MovieSearch.Application.Services.Clients;

namespace MovieSearch.Application.Movies.Features.FindById;

public class FindMovieByIdQueryHandler : IRequestHandler<FindMovieByIdQuery, FindMovieByIdQueryResult>
{
    private readonly IMapper _mapper;
    private readonly IMovieDbServiceClient _movieDbServiceClient;

    public FindMovieByIdQueryHandler(IMovieDbServiceClient movieDbServiceClient, IMapper mapper)
    {
        _movieDbServiceClient = movieDbServiceClient;
        _mapper = mapper;
    }

    public async Task<FindMovieByIdQueryResult> Handle(FindMovieByIdQuery query, CancellationToken cancellationToken)
    {
        Guard.Against.Null(query, nameof(FindMovieByIdQuery));

        var movie = await _movieDbServiceClient.GetMovieByIdAsync(query.Id, cancellationToken);

        if (movie is null)
            throw new MovieNotFoundException(query.Id);

        var result = _mapper.Map<MovieDto>(movie);

        return new FindMovieByIdQueryResult { Movie = result };
    }
}
