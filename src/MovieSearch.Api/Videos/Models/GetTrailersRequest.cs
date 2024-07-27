using System;

namespace MovieSearch.Api.Videos.Models;

public class GetTrailersRequest
{
    public string PageToken { get; set; } = "";
    public int PageSize { get; set; } = 20;
    public string MovieName { get; set; }
    public DateTime? PublishedBefore { get; set; }
    public DateTime? PublishedAfter { get; set; }
}
