using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MovieSearch.Core.Configuration
{
    [DataContract]
    public class ApiConfiguration
    {
        [DataMember( Name = "images" )]
        public ImageConfiguration Images { get; private set; }

        [DataMember( Name = "change_keys" )]
        public IReadOnlyList<string> ChangeKeys { get; private set; }

        public override string ToString()
        {
            if( !string.IsNullOrWhiteSpace( Images?.RootUrl ) )
            {
                return Images.RootUrl;
            }

            return "not set";
        }
    }
}
