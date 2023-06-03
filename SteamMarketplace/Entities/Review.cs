namespace SteamMarketplace.Entities
{
    public class Review : BaseEntity
    {

        public User _User;

        public string Rating { get; set; }

        public string Description { get; set; }


        

    }
}
