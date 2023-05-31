using System.Text.Json.Serialization;

namespace SteamMarketplace.Entities
{
    public class User
    {
        
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public int Wallet { get; set; }
        
        public Dictionary<string, string> Inventory { get; set; }

        public Dictionary<string,string> Transaction_History { get; set; }
    }
}
