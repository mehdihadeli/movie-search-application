using System.Collections.Generic;
using MovieSearch.Core;
using TMDbLib.Objects.Search;

namespace MovieSearch.Application.People.Dtos
{
    public class PersonLookupDto
    {
        public PersonLookupDto()
        {
            MediaType = MediaType.Person;
        }

        public bool Adult { get; init; }

        public List<KnownForBase> KnownFor { get; init; }

        public string Name { get; init; }

        public string ProfilePath { get; init; }
        public MediaType MediaType { get; }
    }
}