using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using MovieSearch.Application.Services.Clients;
using MovieSearch.Application.TvShows.Dtos;

namespace MovieSearch.Application.TvShows.Features.SearchTVShowByTitle
{
    public class
        SearchTVShowByTitleQueryHandler : IRequestHandler<SearchTVShowByTitleQuery, SearchTVShowByTitleQueryResult>
    {
        private readonly IMovieDbServiceClient _movieDbServiceClient;
        private readonly IMapper _mapper;

        public SearchTVShowByTitleQueryHandler(IMovieDbServiceClient movieDbServiceClient, IMapper mapper)
        {
            _movieDbServiceClient = movieDbServiceClient;
            _mapper = mapper;
        }

        public async Task<SearchTVShowByTitleQueryResult> Handle(SearchTVShowByTitleQuery query,
            CancellationToken cancellationToken)
        {
            Guard.Against.Null(query, nameof(SearchTVShowByTitleQuery));

            var tvShows = await _movieDbServiceClient.SearchTvShowAsync(keyword: query.SearchKeywords, page: query
                .Page, cancellationToken: cancellationToken);

            var result = tvShows.Map(x => _mapper.Map<TVShowInfoDto>(x));

            return new SearchTVShowByTitleQueryResult { TVShowList = result };
        }
    }
}