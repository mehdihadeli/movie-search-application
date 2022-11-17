using System;
using System.Collections.Generic;

namespace MovieSearch.Core.People;

public class PersonMovieCredit
{
    public PersonMovieCredit()
    {
        CastRoles = Array.Empty<PersonMovieCastMember>();
        CrewRoles = Array.Empty<PersonMovieCrewMember>();
    }

    public int PersonId { get; init; }
    public IReadOnlyList<PersonMovieCastMember> CastRoles { get; init; }
    public IReadOnlyList<PersonMovieCrewMember> CrewRoles { get; init; }
}

public class PersonMovieCastMember
{
    public int Id { get; init; }
    public bool Adult { get; init; }
    public string Character { get; init; }
    public string CreditId { get; init; }
    public string OriginalTitle { get; init; }
    public string PosterPath { get; init; }
    public DateTime? ReleaseDate { get; init; }
    public string Title { get; init; }
}

public class PersonMovieCrewMember
{
    public int Id { get; init; }
    public bool Adult { get; init; }
    public string CreditId { get; init; }
    public string Department { get; init; }
    public string Job { get; init; }
    public string OriginalTitle { get; init; }
    public string PosterPath { get; init; }
    public DateTime? ReleaseDate { get; init; }
    public string Title { get; init; }
}