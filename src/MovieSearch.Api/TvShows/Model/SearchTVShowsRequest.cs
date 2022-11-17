namespace MovieSearch.Api.TvShows.Model;

public class SearchTVShowsRequest
{
    public int FirstAirDateYear { get; set; }
    public bool IncludeAdult { get; set; }
    public string SearchKeywords { get; set; }
    public int Page { get; set; } = 1;
}