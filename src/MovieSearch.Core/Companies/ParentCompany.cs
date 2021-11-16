
namespace MovieSearch.Core.Companies
{
    public class ParentCompany
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string LogoPath { get; init; }

        public override string ToString()
        {
            if( string.IsNullOrWhiteSpace( Name ) )
            {
                return "n/a";
            }

            return $"{Name} ({Id})";
        }
    }
}
