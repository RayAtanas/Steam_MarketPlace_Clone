using MongoDB.Driver;
using SteamMarketplace.Entities;

namespace SteamMarketplace.Repository
{
    public interface IItemRepository
    {

        Task<User> Get(string id);

        Task<User> Find(FilterDefinition<Item> filter);

        Task Update(Item item);
        Task Delete(Item item);
        Task create(Item item);


    }
}
