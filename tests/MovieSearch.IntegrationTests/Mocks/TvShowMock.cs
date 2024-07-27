using System;
using System.Collections.Generic;
using MovieSearch.Core.Companies;
using MovieSearch.Core.Genres;
using MovieSearch.Core.People;
using MovieSearch.Core.TV;

namespace MovieSearch.IntegrationTests.Mocks;

public static class TvShowMock
{
    public static TVShow Data =>
        new()
        {
            Id = 1399,
            FirstAirDate = new DateTime(2011, 04, 17),
            Homepage = "http://www.hbo.com/game-of-thrones",
            Name = "Game of Thrones",
            OriginalLanguage = "en",
            CreatedBy = new List<TVShowCreator>
            {
                new()
                {
                    Id = 9813,
                    CreditId = "5256c8c219c2956ff604858a",
                    Name = "David Benioff",
                    Gender = Gender.Male,
                    ProfilePath = "/xvNN5huL0X8yJ7h3IZfGG4O2zBD.jpg"
                },
                new()
                {
                    Id = 228068,
                    CreditId = "552e611e9251413fea000901",
                    Name = "D. B. Weiss",
                    Gender = Gender.Male,
                    ProfilePath = "/2RMejaT793U9KRk2IEbFfteQntE.jpg"
                }
            },
            EpisodeRunTime = new[] { 60 },
            Genres = new List<Genre>
            {
                GenreFactory.SciFiAndFantasy(),
                GenreFactory.ActionAndAdventure(),
                GenreFactory.Drama()
            },
            Languages = new[] { "en" },
            Networks = new List<Network> { new(49, "HBO") },
            OriginCountry = new[] { "US" },
            ProductionCompanies = new List<ProductionCompany>
            {
                new()
                {
                    Id = 76043,
                    LogoPath = "/9RO2vbQ67otPrBLXCaC8UMp3Qat.png",
                    Name = "Revolution Sun Studios",
                    OriginCountry = "US"
                },
                new()
                {
                    Id = 12525,
                    LogoPath = null,
                    Name = "Television 360",
                    OriginCountry = ""
                },
                new()
                {
                    Id = 5820,
                    LogoPath = null,
                    Name = "Generator Entertainment",
                    OriginCountry = ""
                },
                new()
                {
                    Id = 12526,
                    LogoPath = null,
                    Name = "Bighead Littlehead",
                    OriginCountry = ""
                }
            }
        };
}
