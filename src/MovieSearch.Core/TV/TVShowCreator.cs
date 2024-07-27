using System.Collections.Generic;
using MovieSearch.Core.People;

namespace MovieSearch.Core.TV;

public class TVShowCreator : IEqualityComparer<TVShowCreator>
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string ProfilePath { get; init; }
    public string CreditId { get; init; }
    public Gender Gender { get; set; }

    public bool Equals(TVShowCreator x, TVShowCreator y)
    {
        return x != null
            && y != null
            && x.Id == y.Id
            && x.Name == y.Name
            && x.Gender == y.Gender
            && x.CreditId == y.CreditId;
    }

    public int GetHashCode(TVShowCreator obj)
    {
        unchecked // Overflow is fine, just wrap
        {
            var hash = 17;
            hash = hash * 23 + obj.Id.GetHashCode();
            hash = hash * 23 + obj.Name.GetHashCode();
            return hash;
        }
    }

    public override bool Equals(object obj)
    {
        if (obj is not TVShowCreator showCreator)
            return false;

        return Equals(this, showCreator);
    }

    public override int GetHashCode()
    {
        return GetHashCode(this);
    }

    public override string ToString()
    {
        return $"{Name} ({Id})";
    }
}
