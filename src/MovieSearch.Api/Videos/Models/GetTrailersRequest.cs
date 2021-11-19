namespace MovieSearch.Api.Videos.Models
{
    public class GetTrailersRequest
    {
        public string PageToken { get; set; } = "";
        public int PageSize { get; set; }
        public string MovieName { get; set; }
    }
}