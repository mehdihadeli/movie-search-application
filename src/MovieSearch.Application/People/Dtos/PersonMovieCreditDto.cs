using System;
using System.Collections.Generic;

namespace MovieSearch.Application.People.Dtos;

public class PersonMovieCreditDto
{
    public int PersonId { get; init; }
    public IReadOnlyList<PersonMovieCastMemberDto> CastRoles { get; init; }
    public IReadOnlyList<PersonMovieCrewMemberDto> CrewRoles { get; init; }
}

public class PersonMovieCastMemberDto
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

public class PersonMovieCrewMemberDto
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