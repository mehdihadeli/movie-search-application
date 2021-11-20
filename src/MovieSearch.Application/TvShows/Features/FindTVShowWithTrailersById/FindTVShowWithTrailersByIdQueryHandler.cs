using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using MovieSearch.Application.Services.Clients;
using MovieSearch.Application.TvShows.Dtos;
using MovieSearch.Application.TvShows.Exceptions;
using MovieSearch.Application.Videos.Dtos;

namespace MovieSearch.Application.TvShows.Features.FindTVShowWithTrailersById
{
    public class FindTVShowWithTrailersByIdQueryHandler : IRequestHandler<FindTVShowWithTrailersByIdQuery,
        FindTVShowWithTrailersByIdQueryResult>
    {
        private readonly IMovieDbServiceClient _movieDbServiceClient;
        private readonly IVideoServiceClient _videoServiceClient;
        private readonly IMapper _mapper;

        public FindTVShowWithTrailersByIdQueryHandler(IMovieDbServiceClient movieDbServiceClient, IVideoServiceClient
            videoServiceClient, IMapper mapper)
        {
            _movieDbServiceClient = movieDbServiceClient;
            _videoServiceClient = videoServiceClient;
            _mapper = mapper;
        }

        public async Task<FindTVShowWithTrailersByIdQueryResult> Handle(FindTVShowWithTrailersByIdQuery query,
            CancellationToken cancellationToken)
        {
            Guard.Against.Null(query, nameof(FindTVShowWithTrailersByIdQuery));

            var tvShow = await _movieDbServiceClient.GetTvShowByIdAsync(query.TvShowId, cancellationToken);

            if (tvShow is null)
                throw new TvShowNotFoundException(query.TvShowId);

            var tvShowDto = _mapper.Map<TVShowDto>(tvShow);

            var trailers = (await _videoServiceClient.GetTrailers(tvShowDto.Name, query.TrailersCount)).Items;
            var trailersDto = _mapper.Map<List<VideoDto>>(trailers);

            return new FindTVShowWithTrailersByIdQueryResult
            {
                TVShowWithTrailers = new TVShowWithTrailersDto()
                {
                    TVShow = tvShowDto,
                    Trailers = trailersDto
                }
            };
        }
    }
}