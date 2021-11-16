using System.Collections.Generic;

namespace MovieSearch.Core.Keywords
{
    public class Keyword : IEqualityComparer<Keyword>
    {
        /// <summary>
        /// The keyword Id as identified by theMovieDB.org.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// The keyword.
        /// </summary>
        public string Name { get; init; }

        public Keyword( int id, string name )
        {
            Id = id;
            Name = name;
        }

        public override bool Equals( object obj )
        {
            if( obj is not Keyword genre )
            {
                return false;
            }

            return Equals( this, genre );
        }

        public bool Equals( Keyword x, Keyword y )
            => x != null && y != null && x.Id == y.Id && x.Name == y.Name;

        public override int GetHashCode()
            => GetHashCode( this );

        public int GetHashCode( Keyword obj )
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + obj.Id.GetHashCode();
                hash = hash * 23 + obj.Name.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
            => $"{Name} ({Id})";
    }
}
