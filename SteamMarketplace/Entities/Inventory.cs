namespace SteamMarketplace.Entities
{
    public class Inventory : BaseEntity
    {
        public bool IsPrivate { get; set; }

        public Dictionary<string,Item> items { get; set; }
    }
}
