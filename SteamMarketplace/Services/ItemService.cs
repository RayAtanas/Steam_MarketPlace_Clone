using AutoMapper;
using MongoDB.Driver;
using NPOI.SS.Formula.Functions;
using SteamMarketplace.Entities;
using SteamMarketplace.Entities.DTO;
using SteamMarketplace.Entities.Response;
using SteamMarketplace.Repository;
using System.Net;

namespace SteamMarketplace.Services
{
    public class ItemService
    {
        private Item Item { get; set; }

        private ItemRepository repository { get; set; }
        private readonly IMapper mapper;
        public MongoRepository _mongoRepository;

        private Response response { get; set; }

   

        public ItemService(ItemRepository _repository,IMapper _mapper,MongoRepository mongoRepository)
        {
            repository = _repository;
            mapper = _mapper;
            _mongoRepository = mongoRepository;
        }

        public async Task<Response> CreateItem(ItemDTO itemDTO)
        {
            FilterDefinition<Item> filter = Builders<Item>.Filter.Eq(item => item.Title, itemDTO.title);

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
                    newItem.Title,
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
                Title = itemDTO.title,
                Type = itemDTO.type,
                Description = itemDTO.description,
                Reviews = itemDTO.reviews,
                PaymentState = itemDTO.paymentState,
                Languages = itemDTO.languages,
                Publisher = itemDTO.publisher,
                Developer = itemDTO.developer,
               


            };
        }
        public async Task<Response> GetItemById(string ItemId)
        {

            var items = await _mongoRepository.FindAsync<Item>(c => c.Id == ItemId);

            var model = mapper.Map<ItemDTO>(items);

            if (items == null)
            {
                return new Response()
                {
                    HttpStatus = (int)HttpStatusCode.NotFound,
                    Message = "Item doesn't exist"
                };

            }

            return new Response()
            {
                Message = "success",
                HttpStatus = (int)HttpStatusCode.OK,
                Data = model

            };

        }

        public async Task<Response> FindItemByName(string title)
        {
            try
            {
                List<Item> items = new(
               await _mongoRepository.FindAllAsync<Item>(item => item.Title.ToLower().StartsWith(title.ToLower()))
                 );

                List<ItemDTO> model = mapper.Map<List<Item>, List<ItemDTO>>(items);


                if (items == null)
                {
                    return new Response()
                    {
                        HttpStatus = (int)HttpStatusCode.NotFound,
                        Message = "Item doesn't exist"
                    };
                }

                    return new Response()
                {
                    Data = model,

                    HttpStatus = 200
                };
            }
            catch (Exception e)
            {
                return new Response()
                {
                    Message = e.Message,

                    HttpStatus = (int)HttpStatusCode.InternalServerError
                };
            }

        }

    }
 }
