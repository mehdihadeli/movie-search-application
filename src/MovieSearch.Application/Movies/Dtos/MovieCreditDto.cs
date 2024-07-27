using System.Collections.Generic;
using MovieSearch.Core.People;

namespace MovieSearch.Application.Movies.Dtos;

public class MovieCreditDto
{
    public int MovieId { get; init; }
    public IReadOnlyList<MovieCastMemberDto> CastMembers { get; init; }
    public IReadOnlyList<MovieCrewMemberDto> CrewMembers { get; init; }
}

public class MovieCastMemberDto
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
}

public class MovieCrewMemberDto
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
}
