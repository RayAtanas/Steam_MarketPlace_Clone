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

            if (item != null)
            {

                return new Response()
                {
                    Message = "Item already exist",

                    HttpStatus = 200

                };

            }

            Item newItem = GetNewItem(itemDTO);
            await repository.create(newItem);
            return new Response()
            {
                HttpStatus = 200,

                Data = new
                {
                    newItem.Name,
                    newItem.Description,
                    newItem.PaymentState,
                    newItem.Id,
                    newItem.Type,
                    newItem.Languages,
                    newItem.Developer,
                    newItem.Publisher,
                    newItem.Reviews
                  

                }


            };
        }

        private Item GetNewItem(ItemDTO itemDTO)
        {
            return new Item()
            {
                Id = Guid.NewGuid().ToString(),
                Name = itemDTO.name,
                Type = itemDTO.type,
                Description = itemDTO.description,
                Reviews = itemDTO.reviews,
                PaymentState = itemDTO.paymentState,
                Languages = itemDTO.languages,
                Publisher = itemDTO.publisher,
                Developer = itemDTO.developer,
               


            };
        }
    }
    }
