using System;
using System.Collections.Generic;

namespace MovieSearch.Application.People.Dtos;

public class PersonTVShowCreditDto
{
    public int PersonId { get; init; }
    public IReadOnlyList<PersonTVShowCastMemberDto> CastRoles { get; init; }
    public IReadOnlyList<PersonTVShowCrewMemberDto> CrewRoles { get; init; }
}

public class PersonTVShowCastMemberDto
{
    public int Id { get; init; }
    public string Character { get; init; }
    public string CreditId { get; init; }
    public int EpisodeCount { get; init; }
    public DateTime FirstAirDate { get; init; }
    public string Name { get; init; }
    public string OriginalName { get; init; }
    public string PosterPath { get; init; }
}

public class PersonTVShowCrewMemberDto
{
    public int Id { get; init; }
    public string CreditId { get; init; }
    public string Department { get; init; }
    public int EpisodeCount { get; init; }
    public DateTime FirstAirDate { get; init; }
    public string Job { get; init; }
    public string Name { get; init; }
    public string OriginalName { get; init; }
    public string PosterPath { get; init; }
}
