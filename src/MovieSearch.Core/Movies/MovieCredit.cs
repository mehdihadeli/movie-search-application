using System.Collections.Generic;
using MovieSearch.Core.People;

namespace MovieSearch.Core.Movies;

public class MovieCredit
{
    public int MovieId { get; init; }
    public IReadOnlyList<MovieCastMember> CastMembers { get; init; }
    public IReadOnlyList<MovieCrewMember> CrewMembers { get; init; }
}

public class MovieCastMember
{
    public int Id { get; init; }
    public int CastId { get; init; }
    public string CreditId { get; init; }
    public string Character { get; init; }
    public Gender Gender { get; init; }
    public bool Adult { get; init; }
    public string Name { get; init; }
    public string KnownForDepartment { get; init; }
    public string OriginalName { get; init; }
    public float Popularity { get; init; }
    public int Order { get; init; }
    public string ProfilePath { get; init; }

    public override string ToString()
    {
        return $"{Character}: {Name}";
    }
}

public class MovieCrewMember
{
    public int Id { get; init; }
    public string CreditId { get; init; }
    public string Department { get; init; }
    public string Job { get; init; }
    public string Name { get; init; }
    public string ProfilePath { get; init; }
    public Gender Gender { get; init; }
    public bool Adult { get; init; }
    public string KnownForDepartment { get; init; }
    public string OriginalName { get; init; }
    public float Popularity { get; init; }

    public override string ToString()
    {
        return $"{Name} | {Department} | {Job}";
    }
}