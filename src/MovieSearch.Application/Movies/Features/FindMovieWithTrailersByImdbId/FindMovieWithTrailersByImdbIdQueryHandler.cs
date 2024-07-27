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

namespace MovieSearch.Application.Movies.Features.FindMovieWithTrailersByImdbId;

//https://docs.microsoft.com/en-us/azure/architecture/patterns/gateway-aggregation
public class FindMovieWithTrailersByImdbIdQueryHandler
    : IRequestHandler<FindMovieWithTrailersByImdbIdQuery, FindMovieWithTrailersByImdbIdQueryResult>
{
    private readonly IMapper _mapper;
    private readonly IMovieDbServiceClient _movieDbServiceClient;
    private readonly IVideoServiceClient _videoServiceClient;

    public FindMovieWithTrailersByImdbIdQueryHandler(
        IMovieDbServiceClient movieDbServiceClient,
        IVideoServiceClient videoServiceClient,
        IMapper mapper
    )
    {
        _movieDbServiceClient = movieDbServiceClient;
        _videoServiceClient = videoServiceClient;
        _mapper = mapper;
    }

    public async Task<FindMovieWithTrailersByImdbIdQueryResult> Handle(
        FindMovieWithTrailersByImdbIdQuery query,
        CancellationToken cancellationToken
    )
    {
        Guard.Against.Null(query, nameof(FindMovieWithTrailersByImdbIdQuery));

        var movie = await _movieDbServiceClient.GetMovieByImdbIdAsync(query.ImdbId, cancellationToken);

        if (movie is null)
            throw new MovieNotFoundException(query.ImdbId);

        var movieDto = _mapper.Map<MovieDto>(movie);

        var trailers = (await _videoServiceClient.GetTrailers(movieDto.Title, query.TrailersCount)).Items;
        var trailersDto = _mapper.Map<List<VideoDto>>(trailers);

        return new FindMovieWithTrailersByImdbIdQueryResult
        {
            MovieWithTrailers = new MovieWithTrailersDto { Movie = movieDto, Trailers = trailersDto }
        };
    }
}
