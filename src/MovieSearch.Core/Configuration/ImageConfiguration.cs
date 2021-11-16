using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MovieSearch.Core.Configuration
{
    [DataContract]
    public class ImageConfiguration
    {
        [DataMember( Name = "base_url" )]
        public string RootUrl { get; private set; }

        [DataMember( Name = "secure_base_url" )]
        public string SecureRootUrl { get; private set; }

        [DataMember( Name = "backdrop_sizes" )]
        public IReadOnlyList<string> BackDrops { get; private set; }

        [DataMember( Name = "logo_sizes" )]
        public IReadOnlyList<string> Logos { get; private set; }

        [DataMember( Name = "poster_sizes" )]
        public IReadOnlyList<string> Posters { get; private set; }

        [DataMember( Name = "profile_sizes" )]
        public IReadOnlyList<string> Profiles { get; private set; }

        [DataMember( Name = "still_sizes" )]
        public IReadOnlyList<string> Stills { get; private set; }

        public override string ToString()
        {
            if( !string.IsNullOrWhiteSpace( RootUrl ) )
            {
                return RootUrl;
            }

            return "not set";
        }
    }
}
