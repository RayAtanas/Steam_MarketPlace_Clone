using MongoDB.Driver;
using SteamMarketplace.Database;
using SteamMarketplace.Entities;

namespace SteamMarketplace.Repository
{
    public class ItemRepository
    {

        public readonly Context _context;

        public ItemRepository(Context context)
        {
            _context = context;
        }

        public async Task create(Item item)
        {
            await _context.items.InsertOneAsync(item);
        }

        public async Task Delete(Item item)
        {
            FilterDefinition<Item> query = Builders<Item>.Filter.Where(x => x.Id == item.Id);

            await _context.items.DeleteOneAsync(query);
        }

        public async Task<Item> Find(FilterDefinition<Item> filter)
        {

            return (await _context.items.FindAsync<Item>(filter)).FirstOrDefault();

        }

        public async Task<Item> Get(string id)
        {
            FilterDefinition<Item> query = Builders<Item>.Filter.Where(x => x.Id == id);

            return (await _context.items.FindAsync<Item>(query)).FirstOrDefault();
        }

        public async Task Update(Item item)
        {

            await _context.items.ReplaceOneAsync(filter: x => x.Id == item.Id, replacement: item);

        }

    }
}
