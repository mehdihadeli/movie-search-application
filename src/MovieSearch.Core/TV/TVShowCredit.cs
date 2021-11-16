using System.Collections.Generic;
using MovieSearch.Core.Movies;

namespace MovieSearch.Core.TV
{
    public class TVShowCredit
    {
        public int TvShowId { get; init; }
        public IReadOnlyList<TVShowCastMember> CastMembers { get; init; }
        public IReadOnlyList<TVShowCrewMember> CrewMembers { get; init; }
    }

    public class TVShowCastMember
    {
        public int PersonId { get; init; }
        public int CastId { get; init; }
        public string CreditId { get; init; }
        public string Character { get; init; }
        public string Name { get; init; }
        public int Order { get; init; }
        public string ProfilePath { get; init; }
        public override string ToString()
            => $"{Character}: {Name}";
    }

    public class TVShowCrewMember
    {
        public int PersonId { get; init; }
        public string CreditId { get; init; }
        public string Department { get; init; }
        public string Job { get; init; }
        public string Name { get; init; }

        public string ProfilePath { get; init; }

        public override string ToString()
            => $"{Name} | {Department} | {Job}";
    }
}