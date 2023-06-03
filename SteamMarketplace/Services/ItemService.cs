using MongoDB.Driver;
using SteamMarketplace.Entities;
using SteamMarketplace.Entities.DTO;
using SteamMarketplace.Entities.Response;
using SteamMarketplace.Repository;

namespace SteamMarketplace.Services
{
    public class ItemService
    {
        private Item Item { get; set; }

        private ItemRepository repository { get; set; }

        private Response response { get; set; }

        public ItemService(ItemRepository _repository)
        {
            repository = _repository;
        }

        public async Task<Response> CreateItem(ItemDTO itemDTO)
        {
            FilterDefinition<Item> filter = Builders<Item>.Filter.Eq(item => item.Name, itemDTO.name);

            var item = await repository.Find(filter);

            return new Response()
            {
                // Add your response properties here
            };
        }
    }
    }
