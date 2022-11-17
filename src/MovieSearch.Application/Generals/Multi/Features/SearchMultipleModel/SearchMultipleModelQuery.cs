using BuildingBlocks.Domain;

namespace MovieSearch.Application.Generals.Multi.Features.SearchMultipleModel;

public class SearchMultipleModelQuery : IQuery<SearchMultipleModelQueryResult>
{
    public string SearchKeywords { get; init; }
    public bool IncludeAdult { get; init; }
    public int Page { get; init; } = 1;
    public int Year { get; init; }
}