﻿using Newtonsoft.Json;

namespace SteamMarketplace.Entities.DTO
{
    public class ItemDTO
    {
        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("description")]
        public string description { get; set; }

        [JsonProperty("reviews")]
        public Dictionary<User, List<Review>> reviews { get; set; }

        [JsonProperty("paymentState")]
        public string paymentState { get; set; }

        [JsonProperty("languages")]
        public List<string> languages { get; set; }

        [JsonProperty("publisher")]
        public string publisher { get; set; }

        [JsonProperty("developer")]
        public string developer { get; set; }

        [JsonProperty("realeaseDate")]
        DateTime? realeaseDate { get; set; }

    }
}
