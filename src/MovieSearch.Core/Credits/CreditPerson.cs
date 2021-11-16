using Newtonsoft.Json;

namespace MovieSearch.Core.Credits
{
    public class CreditPerson
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}