using System;
using System.Collections.Generic;
using MovieSearch.Core.People;

namespace MovieSearch.Application.People.Dtos;

public class PersonDto
{
    public int Id { get; init; }
    public string Name { get; init; }
    public IReadOnlyList<string> AlsoKnownAs { get; init; }
    public string KnownForDepartment { get; set; }
    public bool Adult { get; init; }
    public string Biography { get; init; }
    public DateTime? Birthday { get; init; }
    public DateTime? Deathday { get; init; }
    public Gender Gender { get; init; }
    public string Homepage { get; init; }
    public string ImdbId { get; init; }
    public string PlaceOfBirth { get; init; }
    public double Popularity { get; init; }
    public string ProfilePath { get; init; }
}
