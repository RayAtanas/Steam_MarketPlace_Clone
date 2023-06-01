using Newtonsoft.Json;

namespace SteamMarketplace.Entities.DTO
{
    public class UserDTO
    {
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("email")]
        public string email { get; set; }

        [JsonProperty("password")]
        public string password { get; set; }

        [JsonProperty("wallet")]
        public int wallet { get; set; }

        [JsonProperty("inventory")]
        public Dictionary<string, string> inventory { get; set; }

        [JsonProperty("transactionHistory")]
        public Dictionary<string, string> transaction_History { get; set; }

        [JsonProperty("status")]
        public string status { get; set; }
    }
}
