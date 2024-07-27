using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using MovieSearch.Application.Services.Clients;
using MovieSearch.Application.Videos.Dtos;

namespace MovieSearch.Application.Videos.Features.FindTrailers;

public class FindTrailersQueryHandler : IRequestHandler<FindTrailersQuery, FindTrailersQueryResult>
{
    private readonly IMapper _mapper;
    private readonly IVideoServiceClient _videoServiceClient;

    public FindTrailersQueryHandler(IVideoServiceClient videoServiceClient, IMapper mapper)
    {
        _videoServiceClient = videoServiceClient;
        _mapper = mapper;
    }

    public async Task<FindTrailersQueryResult> Handle(FindTrailersQuery query, CancellationToken cancellationToken)
    {
        Guard.Against.Null(query, nameof(FindTrailersQuery));

        var videos = await _videoServiceClient.GetTrailers(
            query.MovieName,
            query.PageSize,
            query.PageToken,
            query.PublishedAfter,
            query.PublishedBefore
        );

        var result = videos.Map(x => _mapper.Map<VideoDto>(x));

        return new FindTrailersQueryResult { VideoList = result };
    }
}
