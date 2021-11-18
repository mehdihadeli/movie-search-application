namespace MovieSearch.Api.Videos.Models
{
    public class GetVideosRequest
    {
        public string PageToken { get; set; } = "";
        public string MovieName { get; set; }
    }
}