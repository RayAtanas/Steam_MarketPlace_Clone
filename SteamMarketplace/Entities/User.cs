using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SteamMarketplace.Entities
{
    public class User : BaseEntity
    {


        [BsonElement("firstname")]
        public string? FirstName { get; set; }

        [BsonElement("lastName")]
        public string? LastName { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("wallet")]
        public decimal? Wallet { get; set; }

        [BsonElement("inventory")]
        public Dictionary<string, Item>? Inventory { get; set; }

        [BsonElement("transaction_History")]
        public TransactionHistory Transaction_History { get; set; }

        [BsonElement("status")]
        public string? Status { get; set; }

        [BsonElement("isBlocked")]
        public bool IsBlocked { get; set; }

        [BsonElement("timeBlocked")]
        public DateTime TimeBlocked { get; set; }

        [BsonElement("attempt")]
        public int Attempt { get; set; }
    }
}
