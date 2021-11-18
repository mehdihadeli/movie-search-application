using MovieSearch.Core.People;

namespace MovieSearch.Application.TvShows.Dtos
{
    public class TVShowCreatorDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string ProfilePath { get; init; }
        public string CreditId { get; init; }
        public Gender Gender { get; set; }
    }
}