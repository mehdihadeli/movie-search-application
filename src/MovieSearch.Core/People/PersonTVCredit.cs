using System;
using System.Collections.Generic;

namespace MovieSearch.Core.People
{
    public class PersonTVCredit
    {
        public int PersonId { get; init; }
        public IReadOnlyList<PersonTVCastMember> CastRoles { get; init; }
        public IReadOnlyList<PersonTVCrewMember> CrewRoles { get; init; }
        public PersonTVCredit()
        {
            CastRoles = Array.Empty<PersonTVCastMember>();
            CrewRoles = Array.Empty<PersonTVCrewMember>();
        }
    }

    public class PersonTVCastMember
    {
        public int TVShowId { get; init; }
        public string Character { get; init; }
        public string CreditId { get; init; }
        public int EpisodeCount { get; init; }
        public DateTime FirstAirDate { get; init; }
        public string Name { get; init; }
        public string OriginalName { get; init; }
        public string PosterPath { get; init; }
    }

    public class PersonTVCrewMember
    {
        public int TVShowId { get; init; }
        public string CreditId { get; init; }
        public string Department { get; init; }
        public int EpisodeCount { get; init; }
        public DateTime FirstAirDate { get; init; }
        public string Job { get; init; }
        public string Name { get; init; }
        public string OriginalName { get; init; }
        public string PosterPath { get; init; }
    }
}
