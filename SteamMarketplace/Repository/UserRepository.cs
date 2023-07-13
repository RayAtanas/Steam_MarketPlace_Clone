using MongoDB.Bson;
using MongoDB.Driver;
using SteamMarketplace.Database;
using SteamMarketplace.Entities;
using Newtonsoft.Json;
namespace SteamMarketplace.Repository
{
    public class UserRepository
    {

        public readonly Context _context;

        public UserRepository(Context context)
        {
            _context = context;
        }

        public async Task create(User user)
        {
            await _context.users.InsertOneAsync(user);
        }

        public async Task Delete(User user)
        {
            FilterDefinition<User> query = Builders<User>.Filter.Where(x => x.Id == user.Id);

            await _context.users.DeleteOneAsync(query);
        }

        public async Task<User> Find(FilterDefinition<User> filter)
        {

            return (await _context.users.FindAsync<User>(filter)).FirstOrDefault();

        }
        public async Task<User> Findd(FilterDefinition<User> filter)
        {
           
                return await _context.users.Find(filter).SingleOrDefaultAsync();
            

        }

        public async Task<User> FindByEmail(string email)
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Eq(u => u.Email, email);
            User user = await _context.users.Find(filter).SingleOrDefaultAsync();
            return user;
        }

        public async Task<User> Get(string id)
        {
            FilterDefinition<User> query = Builders<User>.Filter.Eq(x => x.Id, id);

            return await _context.users.Find(query).SingleOrDefaultAsync();
        }


        public async Task Update(User user)
        {

            await _context.users.ReplaceOneAsync(filter: x => x.Id == user.Id, replacement: user);

        }

    }
}
