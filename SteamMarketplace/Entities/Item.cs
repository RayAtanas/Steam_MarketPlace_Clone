namespace SteamMarketplace.Entities
{
    public class Item : BaseEntity
    {
        public string Type { get; set; }
      
        public string Title { get; set; }

        public string Description { get; set; }

        public Dictionary<User, List<Review>> Reviews { get; set; }

        public string PaymentState { get; set; }
        
        public List<string> Languages { get; set; }
    
        public string Publisher { get; set; }

        public string Developer { get; set; }

        DateTime? RealeaseDate { get; set; }


    }
}
