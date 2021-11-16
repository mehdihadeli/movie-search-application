using System.Collections.Generic;

namespace MovieSearch.Core.Generals
{
    public class Country : IEqualityComparer<Country>
    {
        public string Iso3166Code { get; init; }
        public string Name { get; init; }

        public Country( string iso3166Code, string name )
        {
            Iso3166Code = iso3166Code;
            Name = name;
        }

        public override bool Equals( object obj )
        {
            if( obj is not Country country )
            {
                return false;
            }

            return Equals( this, country );
        }

        public bool Equals( Country x, Country y )
            => x != null && y != null && x.Iso3166Code == y.Iso3166Code && x.Name == y.Name;

        public override int GetHashCode() =>
            GetHashCode( this );

        public int GetHashCode( Country obj )
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + obj.Iso3166Code.GetHashCode();
                hash = hash * 23 + obj.Name.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            if( string.IsNullOrWhiteSpace( Name ) )
            {
                return "n/a";
            }

            return $"{Name} ({Iso3166Code})";
        }
    }
}
