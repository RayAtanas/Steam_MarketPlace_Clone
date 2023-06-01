using SteamMarketplace.Entities;
using MongoDB.Driver;
namespace SteamMarketplace.Repository
{
    public interface IMongoRepository
    {
        Task<User> Get(string id);

        Task<User> Find(FilterDefinition<User> filter);

        Task Update(User user);
        Task Delete(User user);
        Task create(User user);
    }
}
