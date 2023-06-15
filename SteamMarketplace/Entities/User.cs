using System.Text.Json.Serialization;

namespace SteamMarketplace.Entities
{
    public class User : BaseEntity
    {
        
      

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public decimal? Wallet { get; set; }
        
        public Dictionary<string, Item>? Inventory { get; set; }

        public TransactionHistory Transaction_History { get; set; }

        public string? Status { get; set; }

        public bool IsBlocked { get; set; }

        public DateTime TimeBlocked { get; set; }

        public int Attempt { get; set; }
    }
}
