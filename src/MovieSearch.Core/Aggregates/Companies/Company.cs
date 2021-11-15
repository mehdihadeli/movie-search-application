namespace MovieSearch.Core.Aggregates.Companies
{
    public class Company
    {
        public string Description { get; init; }
        public string Headquarters { get; init; }
        public string Homepage { get; init; }
        public int Id { get; init; }
        public string LogoPath { get; init; }
        public string Name { get; init; }
        public Company ParentCompany { get; init; }
        public string OriginCountry { get; init; }
    }
}