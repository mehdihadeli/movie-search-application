using System;
using MovieSearch.Core.People;

namespace MovieSearch.EndToEndTests.Mocks;

public static class PersonMock
{
    public static Person Data => new()
    {
        Id = 63,
        Name = "Milla Jovovich",
        Biography =
            "Milla Jovovich (born December 17, 1975) is an Ukrainian-born American actress, supermodel, musician, and fashion designer.", //truncated
        Birthday = DateTime.Parse("1975-12-17"),
        Gender = Gender.Female,
        Homepage = "http://www.millaj.com",
        ImdbId = "nm0000170",
        PlaceOfBirth = "Kiev, Ukraine"
    };
}