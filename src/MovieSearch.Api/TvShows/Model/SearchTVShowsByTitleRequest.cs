namespace MovieSearch.Api.TvShows.Model;

public class SearchTVShowsByTitleRequest
{
    public int Page { get; set; } = 1;
    public string SearchKeywords { get; set; }
}
