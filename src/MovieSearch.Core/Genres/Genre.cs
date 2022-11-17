using System.Collections.Generic;

namespace MovieSearch.Core.Genres;

public class Genre : IEqualityComparer<Genre>
{
    public Genre(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; init; }

    public string Name { get; init; }

    public bool Equals(Genre x, Genre y)
    {
        return x != null && y != null && x.Id == y.Id && x.Name == y.Name;
    }

    public int GetHashCode(Genre obj)
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
        if (obj is not Genre genre) return false;

        return Equals(this, genre);
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