namespace MovieSearch.Api.Multi.Models
{
    public class SearchMultipleModelRequest
    {
        public string SearchKeywords { get; set; }
        public bool IncludeAdult { get; set; }
        public int Page { get; set; } = 1;
        public int Year { get; set; }
    }
}