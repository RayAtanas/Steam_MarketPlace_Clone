using MongoDB.Driver;
using SteamMarketplace.Entities;

namespace SteamMarketplace.Repository
{
    public interface IUserRepository
    {

        Task<User> Get(string id);

        Task<User> Find(FilterDefinition<User> filter);

        Task Update(User user);
        Task Delete(User user);
        Task create(User user);


    }
}
