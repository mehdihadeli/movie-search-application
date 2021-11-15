using BuildingBlocks.Domain;

namespace MovieSearch.Core.Aggregates.Credits
{
    public class Credit : IAggregate<string>
    {
        public string Id { get; init; }
        public CreditType CreditType { get; init; }
        public string Department { get; init; }
        public string Job { get; init; }
        public CreditMedia Media { get; init; }
        public MediaType MediaType { get; init; }
        public CreditPerson Person { get; init; }
    }
}