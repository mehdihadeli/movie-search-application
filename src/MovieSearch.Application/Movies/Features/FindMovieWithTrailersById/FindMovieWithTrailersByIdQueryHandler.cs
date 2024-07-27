using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using MovieSearch.Application.Movies.Dtos;
using MovieSearch.Application.Movies.Exceptions;
using MovieSearch.Application.Services.Clients;
using MovieSearch.Application.Videos.Dtos;

namespace MovieSearch.Application.Movies.Features.FindMovieWithTrailersById;

//https://docs.microsoft.com/en-us/azure/architecture/patterns/gateway-aggregation
public class FindMovieWithTrailersByIdQueryHandler
    : IRequestHandler<FindMovieWithTrailersByIdQuery, FindMovieWithTrailersByIdQueryResult>
{
    private readonly IMapper _mapper;
    private readonly IMovieDbServiceClient _movieDbServiceClient;
    private readonly IVideoServiceClient _videoServiceClient;

    public FindMovieWithTrailersByIdQueryHandler(
        IMovieDbServiceClient movieDbServiceClient,
        IVideoServiceClient videoServiceClient,
        IMapper mapper
    )
    {
        _movieDbServiceClient = movieDbServiceClient;
        _videoServiceClient = videoServiceClient;
        _mapper = mapper;
    }

    public async Task<FindMovieWithTrailersByIdQueryResult> Handle(
        FindMovieWithTrailersByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        Guard.Against.Null(query, nameof(FindMovieWithTrailersByIdQuery));

        var movie = await _movieDbServiceClient.GetMovieByIdAsync(query.MovieId, cancellationToken);

        if (movie is null)
            throw new MovieNotFoundException(query.MovieId);

        var movieDto = _mapper.Map<MovieDto>(movie);

        var trailers = (await _videoServiceClient.GetTrailers(movieDto.Title, query.TrailersCount)).Items;
        var trailersDto = _mapper.Map<List<VideoDto>>(trailers);

        return new FindMovieWithTrailersByIdQueryResult(
            new MovieWithTrailersDto { Movie = movieDto, Trailers = trailersDto }
        );
    }
}
