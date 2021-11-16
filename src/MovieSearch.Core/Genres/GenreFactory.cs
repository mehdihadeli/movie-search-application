using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MovieSearch.Core.Genres
{
    public static class GenreFactory
    {
        public static Genre Action()
            => new( 28, "Action" );

        public static Genre Adventure()
            => new( 12, "Adventure" );

        public static Genre ActionAndAdventure()
            => new( 10759, "Action & Adventure" );

        public static Genre Animation()
            => new( 16, "Animation" );

        public static Genre Comedy()
            => new( 35, "Comedy" );

        public static Genre Crime()
            => new( 80, "Crime" );

        public static Genre Drama()
            => new( 18, "Drama" );

        public static Genre Documentary()
            => new( 99, "Documentary" );

        public static Genre Family()
            => new( 10751, "Family" );

        public static Genre Fantasy()
            => new( 14, "Fantasy" );

        public static Genre History()
            => new( 36, "History" );

        public static Genre Horror()
            => new( 27, "Horror" );

        public static Genre Kids()
            => new( 10762, "Kids" );

        public static Genre Music()
            => new( 10402, "Music" );

        public static Genre Mystery()
            => new( 9648, "Mystery" );

        public static Genre News()
            => new( 10763, "News" );

        public static Genre Reality()
            => new( 10764, "Reality" );

        public static Genre Romance()
            => new( 10749, "Romance" );

        public static Genre ScienceFiction()
            => new( 878, "Science Fiction" );

        public static Genre SciFiAndFantasy()
            => new( 10765, "Sci-Fi & Fantasy" );

        public static Genre Soap()
            => new( 10766, "Soap" );

        public static Genre Talk()
            => new( 10767, "Talk" );

        public static Genre Thriller()
            => new( 53, "Thriller" );

        public static Genre TvMovie()
            => new( 10770, "TV Movie" );

        public static Genre War()
            => new( 10752, "War" );

        public static Genre WarAndPolitics()
            => new( 10768, "War & Politics" );

        public static Genre Western()
            => new( 37, "Western" );

        public static IReadOnlyList<Genre> GetAll()
            => LazyAll.Value;


        private static readonly Lazy<IReadOnlyList<Genre>> LazyAll = new( () =>
        {
            var all = typeof( GenreFactory )
                .GetTypeInfo()
                .DeclaredMethods
                .Where( x => x.IsStatic )
                .Where( x => x.IsPublic )
                .Where( x => x.ReturnType == typeof( Genre ) )
                .Select( x => ( Genre )x.Invoke( null, null ) )
                .ToList();

            return all.AsReadOnly();
        } );
    }
}
