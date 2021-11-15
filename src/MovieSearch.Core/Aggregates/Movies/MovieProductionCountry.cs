namespace MovieSearch.Core.Aggregates.Movies
{
    public class MovieProductionCountry
    {
        public MovieProductionCountry(string name, string iso31661)
        {
            Iso_3166_1 = iso31661;
            Name = name;
        }

        public string Iso_3166_1 { get; }
        public string Name { get; }
    }
}