using Newtonsoft.Json;

namespace SteamMarketplace.Entities.DTO
{
    public class ReviewDTO
    {

        [JsonProperty("rating")]
        public string rating { get; set; }

        [JsonProperty("rating")]
        public string description { get; set; }
    }
}
