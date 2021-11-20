using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using MovieSearch.Application.Services.Clients;
using MovieSearch.Application.TvShows.Exceptions;
using MovieSearch.Application.TvShows.Features.FindTvShowById;
using MovieSearch.Application.Videos.Dtos;

namespace MovieSearch.Application.Videos.Features.FindTVShowTrailers
{
    public class
        FindTVShowTrailersQueryHandler : IRequestHandler<FindTVShowTrailersQuery, FindTVShowTrailersQueryResult>
    {
        private readonly IMovieDbServiceClient _movieDbServiceClient;
        private readonly IVideoServiceClient _videoServiceClient;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public FindTVShowTrailersQueryHandler(IMovieDbServiceClient movieDbServiceClient,
            IVideoServiceClient videoServiceClient, IMapper mapper, IMediator mediator)
        {
            _movieDbServiceClient = movieDbServiceClient;
            _videoServiceClient = videoServiceClient;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<FindTVShowTrailersQueryResult> Handle(FindTVShowTrailersQuery query,
            CancellationToken cancellationToken)
        {
            Guard.Against.Null(query, nameof(FindTVShowTrailersQuery));

            var tvShow = (await _mediator.Send(new FindTvShowByIdQuery(query.TVShowId), cancellationToken)).TvShow;

            if (tvShow is null)
                throw new TvShowNotFoundException(query.TVShowId);

            var videos = await _videoServiceClient.GetTrailers(tvShow.Name, query.PageSize, query.PageToken,
                tvShow.FirstAirDate);

            var result = videos.Map(x => _mapper.Map<VideoDto>(x));

            return new FindTVShowTrailersQueryResult { VideoList = result };
        }
    }
}