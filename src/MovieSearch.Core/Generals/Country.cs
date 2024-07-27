using System.Collections.Generic;

namespace MovieSearch.Core.Generals;

public class Country : IEqualityComparer<Country>
{
    public string Iso3166Code { get; init; }
    public string Name { get; init; }

    public bool Equals(Country x, Country y)
    {
        return x != null && y != null && x.Iso3166Code == y.Iso3166Code && x.Name == y.Name;
    }

    public int GetHashCode(Country obj)
    {
        unchecked // Overflow is fine, just wrap
        {
            var hash = 17;
            hash = hash * 23 + obj.Iso3166Code.GetHashCode();
            hash = hash * 23 + obj.Name.GetHashCode();
            return hash;
        }
    }

    public override bool Equals(object obj)
    {
        if (obj is not Country country)
            return false;

        return Equals(this, country);
    }

    public override int GetHashCode()
    {
        return GetHashCode(this);
    }

    public override string ToString()
    {
        if (string.IsNullOrWhiteSpace(Name))
            return "n/a";

        return $"{Name} ({Iso3166Code})";
    }
}
