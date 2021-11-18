using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using MovieSearch.Application.Services.Clients;
using MovieSearch.Application.Videos.Dtos;

namespace MovieSearch.Application.Videos.FindVideos
{
    public class FindVideosQueryHandler : IRequestHandler<FindVideosQuery, FindVideosQueryResult>
    {
        private readonly IVideoServiceClient _videoServiceClient;
        private readonly IMapper _mapper;

        public FindVideosQueryHandler(IVideoServiceClient videoServiceClient, IMapper mapper)
        {
            _videoServiceClient = videoServiceClient;
            _mapper = mapper;
        }

        public async Task<FindVideosQueryResult> Handle(FindVideosQuery query, CancellationToken cancellationToken)
        {
            Guard.Against.Null(query, nameof(FindVideosQuery));

            var videos = await _videoServiceClient.GetVideos(query.MovieName, 20, query.PageToken);

            var result = videos.Map(x => _mapper.Map<VideoDto>(x));

            return new FindVideosQueryResult { VideoList = result };
        }
    }
}