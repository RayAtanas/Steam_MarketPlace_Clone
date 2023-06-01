using MongoDB.Driver;
using SteamMarketplace.Entities;
using SteamMarketplace.Database;
namespace SteamMarketplace.Repository
{
    public class MongoRepository
    {
        public readonly Context _context;

        public MongoRepository(Context context)
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

        public async Task<User> Get(string id)
        {
            FilterDefinition<User> query = Builders<User>.Filter.Where(x => x.Id == id);

            return (await _context.users.FindAsync<User>(query)).FirstOrDefault();
        }

        public async Task Update(User user)
        {

            await _context.users.ReplaceOneAsync(filter: x => x.Id == user.Id, replacement: user);

        }
    }
}
