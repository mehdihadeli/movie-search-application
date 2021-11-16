using MovieSearch.Core;
using MovieSearch.Core.Generals;
using Newtonsoft.Json;

namespace MovieSearch.Core.Credits
{
    public class Credit
    {
        [JsonProperty("credit_type")]
        public CreditType CreditType { get; set; }

        [JsonProperty("department")]
        public string Department { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("job")]
        public string Job { get; set; }

        [JsonProperty("media")]
        public CreditMedia Media { get; set; }

        [JsonProperty("media_type")]
        public MediaType MediaType { get; set; }

        [JsonProperty("person")]
        public CreditPerson Person { get; set; }
    }
}