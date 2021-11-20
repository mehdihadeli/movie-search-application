namespace MovieSearch.Api.Videos.Models
{
    public class GetMovieTrailersRequest
    {
        public string PageToken { get; set; } = "";
        public int PageSize { get; set; } = 20;
        public int MovieId { get; set; }
    }
}