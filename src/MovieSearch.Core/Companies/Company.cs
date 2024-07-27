namespace MovieSearch.Core.Companies;

public class Company
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string Headquarters { get; init; }
    public string Homepage { get; init; }
    public string LogoPath { get; init; }
    public string OriginCountry { get; init; }
    public ParentCompany ParentCompany { get; init; }

    public override string ToString()
    {
        return $"{Name} ({Id})";
    }
}
