using Newtonsoft.Json;

namespace SteamMarketplace.Entities.DTO
{
    public class UserDTO
    {
       

        [JsonProperty("firstname")]
        public string? firstname { get; set; }

        [JsonProperty("lastname")]
        public string? lastname { get; set; }

        [JsonProperty("email")]
        public string email { get; set; }

        [JsonProperty("password")]
        public string password { get; set; }

        [JsonProperty("wallet")]
        public decimal? wallet { get; set; }

        [JsonProperty("inventory")]
        public Dictionary<string,Item>? inventory { get; set; }
        
       
        [JsonProperty("status")]
        public string? status { get; set; }
    }
}
