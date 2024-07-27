using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using MovieSearch.Application.Services.Clients;
using MovieSearch.Application.TvShows.Dtos;
using MovieSearch.Application.TvShows.Exceptions;

namespace MovieSearch.Application.TvShows.Features.FindTvShowById;

public class FindTvShowByIdQueryHandler : IRequestHandler<FindTvShowByIdQuery, FindTvShowByIdQueryResult>
{
    private readonly IMapper _mapper;
    private readonly IMovieDbServiceClient _movieDbServiceClient;

    public FindTvShowByIdQueryHandler(IMovieDbServiceClient movieDbServiceClient, IMapper mapper)
    {
        _movieDbServiceClient = movieDbServiceClient;
        _mapper = mapper;
    }

    public async Task<FindTvShowByIdQueryResult> Handle(FindTvShowByIdQuery query, CancellationToken cancellationToken)
    {
        Guard.Against.Null(query, nameof(FindTvShowByIdQuery));

        var tvShow = await _movieDbServiceClient.GetTvShowByIdAsync(query.TvShowId, cancellationToken);

        if (tvShow is null)
            throw new TvShowNotFoundException(query.TvShowId);

        var result = _mapper.Map<TVShowDto>(tvShow);

        return new FindTvShowByIdQueryResult { TvShow = result };
    }
}
