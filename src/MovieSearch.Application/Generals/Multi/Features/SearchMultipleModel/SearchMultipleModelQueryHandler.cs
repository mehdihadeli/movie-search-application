using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using MovieSearch.Application.Services.Clients;

namespace MovieSearch.Application.Generals.Multi.Features.SearchMultipleModel;

public class SearchMultipleModelQueryHandler : IRequestHandler<SearchMultipleModelQuery, SearchMultipleModelQueryResult>
{
    private readonly IMapper _mapper;
    private readonly IMovieDbServiceClient _movieDbServiceClient;

    public SearchMultipleModelQueryHandler(IMovieDbServiceClient movieDbServiceClient, IMapper mapper)
    {
        _movieDbServiceClient = movieDbServiceClient;
        _mapper = mapper;
    }

    public async Task<SearchMultipleModelQueryResult> Handle(
        SearchMultipleModelQuery query,
        CancellationToken cancellationToken
    )
    {
        Guard.Against.Null(query, nameof(SearchMultipleModelQuery));

        var searchData = await _movieDbServiceClient.SearchMultiAsync(
            query.SearchKeywords,
            query.Page,
            query.IncludeAdult,
            query.Year,
            cancellationToken
        );

        return new SearchMultipleModelQueryResult { List = searchData };
    }
}
