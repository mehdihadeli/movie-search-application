using System.Collections.Generic;

namespace MovieSearch.Core.Genres
{
    public class Genre : IEqualityComparer<Genre>
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public Genre( int id, string name )
        {
            Id = id;
            Name = name;
        }

        public override bool Equals( object obj )
        {
            if( obj is not Genre genre )
            {
                return false;
            }

            return Equals( this, genre );
        }

        public bool Equals( Genre x, Genre y )
            => x != null && y != null && x.Id == y.Id && x.Name == y.Name;

        public override int GetHashCode()
            => GetHashCode( this );

        public int GetHashCode( Genre obj )
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
