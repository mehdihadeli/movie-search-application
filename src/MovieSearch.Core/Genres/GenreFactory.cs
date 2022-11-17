using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MovieSearch.Core.Genres;

public static class GenreFactory
{
    private static readonly Lazy<IReadOnlyList<Genre>> LazyAll = new(() =>
    {
        var all = typeof(GenreFactory)
            .GetTypeInfo()
            .DeclaredMethods
            .Where(x => x.IsStatic)
            .Where(x => x.IsPublic)
            .Where(x => x.ReturnType == typeof(Genre))
            .Select(x => (Genre) x.Invoke(null, null))
            .ToList();

        return all.AsReadOnly();
    });

    public static Genre Action()
    {
        return new(28, "Action");
    }

    public static Genre Adventure()
    {
        return new(12, "Adventure");
    }

    public static Genre ActionAndAdventure()
    {
        return new(10759, "Action & Adventure");
    }

    public static Genre Animation()
    {
        return new(16, "Animation");
    }

    public static Genre Comedy()
    {
        return new(35, "Comedy");
    }

    public static Genre Crime()
    {
        return new(80, "Crime");
    }

    public static Genre Drama()
    {
        return new(18, "Drama");
    }

    public static Genre Documentary()
    {
        return new(99, "Documentary");
    }

    public static Genre Family()
    {
        return new(10751, "Family");
    }

    public static Genre Fantasy()
    {
        return new(14, "Fantasy");
    }

    public static Genre History()
    {
        return new(36, "History");
    }

    public static Genre Horror()
    {
        return new(27, "Horror");
    }

    public static Genre Kids()
    {
        return new(10762, "Kids");
    }

    public static Genre Music()
    {
        return new(10402, "Music");
    }

    public static Genre Mystery()
    {
        return new(9648, "Mystery");
    }

    public static Genre News()
    {
        return new(10763, "News");
    }

    public static Genre Reality()
    {
        return new(10764, "Reality");
    }

    public static Genre Romance()
    {
        return new(10749, "Romance");
    }

    public static Genre ScienceFiction()
    {
        return new(878, "Science Fiction");
    }

    public static Genre SciFiAndFantasy()
    {
        return new(10765, "Sci-Fi & Fantasy");
    }

    public static Genre Soap()
    {
        return new(10766, "Soap");
    }

    public static Genre Talk()
    {
        return new(10767, "Talk");
    }

    public static Genre Thriller()
    {
        return new(53, "Thriller");
    }

    public static Genre TvMovie()
    {
        return new(10770, "TV Movie");
    }

    public static Genre War()
    {
        return new(10752, "War");
    }

    public static Genre WarAndPolitics()
    {
        return new(10768, "War & Politics");
    }

    public static Genre Western()
    {
        return new(37, "Western");
    }

    public static IReadOnlyList<Genre> GetAll()
    {
        return LazyAll.Value;
    }
}