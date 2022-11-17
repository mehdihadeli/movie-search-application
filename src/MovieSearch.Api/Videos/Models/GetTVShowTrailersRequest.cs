namespace MovieSearch.Api.Videos.Models;

public class GetTVShowTrailersRequest
{
    public string PageToken { get; set; } = "";
    public int PageSize { get; set; } = 20;
    public int TVShowId { get; set; }
}