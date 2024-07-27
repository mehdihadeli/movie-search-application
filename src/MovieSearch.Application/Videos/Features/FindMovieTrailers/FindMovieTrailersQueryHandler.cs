using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using MovieSearch.Application.Movies.Exceptions;
using MovieSearch.Application.Movies.Features.FindById;
using MovieSearch.Application.Services.Clients;
using MovieSearch.Application.Videos.Dtos;

namespace MovieSearch.Application.Videos.Features.FindMovieTrailers;

public class FindMovieTrailersQueryHandler : IRequestHandler<FindMovieTrailersQuery, FindMovieTrailersQueryResult>
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IVideoServiceClient _videoServiceClient;

    public FindMovieTrailersQueryHandler(IVideoServiceClient videoServiceClient, IMapper mapper, IMediator mediator)
    {
        _videoServiceClient = videoServiceClient;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<FindMovieTrailersQueryResult> Handle(
        FindMovieTrailersQuery query,
        CancellationToken cancellationToken
    )
    {
        Guard.Against.Null(query, nameof(FindMovieTrailersQuery));

        var movie = (await _mediator.Send(new FindMovieByIdQuery { Id = query.MovieId }, cancellationToken)).Movie;

        if (movie is null)
            throw new MovieNotFoundException(query.MovieId);

        var videos = await _videoServiceClient.GetTrailers(
            movie.Title,
            query.PageSize,
            query.PageToken,
            movie.ReleaseDate
        );

        var result = videos.Map(x => _mapper.Map<VideoDto>(x));

        return new FindMovieTrailersQueryResult { VideoList = result };
    }
}
