namespace Thesaurus.Api.Words.ViewModels;

public class SearchMoviesRequest
{
    public int Year { get; set; }
    public int PrimaryReleaseYear { get; set; }
    public bool IncludeAdult { get; set; }
    public string SearchKeywords { get; set; }
    public int Page { get; set; } = 1;
}
