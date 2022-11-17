using System.Collections.Generic;
using MovieSearch.Core.People;

namespace MovieSearch.Application.TvShows.Dtos;

public class TVShowCreditDto
{
    public int TvShowId { get; init; }
    public IReadOnlyList<TVShowCastMemberDto> CastMembers { get; init; }
    public IReadOnlyList<TVShowCrewMemberDto> CrewMembers { get; init; }
}

public class TVShowCastMemberDto
{
    public int Id { get; init; }
    public string CreditId { get; init; }
    public string Character { get; init; }
    public Gender Gender { get; init; }
    public string Name { get; init; }
    public bool Adult { get; init; }
    public int Order { get; init; }
    public string ProfilePath { get; init; }
    public string KnownForDepartment { get; init; }
    public string OriginalName { get; init; }
    public float Popularity { get; init; }
}

public class TVShowCrewMemberDto
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