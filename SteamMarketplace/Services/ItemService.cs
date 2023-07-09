using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using SteamMarketplace.Entities;
using SteamMarketplace.Entities.DTO;
using SteamMarketplace.Entities.Response;
using SteamMarketplace.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace SteamMarketplace.Services
{
    public class ItemService
    {
        private Item Item { get; set; }
        private User User { get; set; }
        private ItemRepository repository { get; set; }
        private readonly IMapper mapper;
        public UserRepository userrepository;
        private Inventory inventory { get; set; }
        private Response response { get; set; }


        private readonly SymmetricSecurityKey _secretkey;
        public ItemService(ItemRepository _repository, IMapper _mapper, UserRepository _userrepository, SymmetricSecurityKey secretkey)
        {
            repository = _repository;
            mapper = _mapper;
            userrepository = _userrepository;
            _secretkey = secretkey;
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
                    newItem.ItemId,
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
                ItemId = Guid.NewGuid().ToString(),
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

            FilterDefinition<Item> filter = Builders<Item>.Filter.Eq(item => item.ItemId, ItemId);

            var item = await repository.Find(filter);

            if (item == null)
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
                Data = item

            };

        }

        public async Task<Response> FindItemByName(string title)
        {
            try
            {

                FilterDefinition<Item> filter = Builders<Item>.Filter.Regex(item => item.Title,
                    new BsonRegularExpression($"^{Regex.Escape(title)}", "i"));



                var item = await repository.Find(filter);

                return new Response()
                {
                    Data = item,

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

        public async Task<Response> ItemPurchase(string title,string userEmail, [FromHeader] string authorization, HttpContext httpContext)
        {
            try
            {

               
                var user = await userrepository.Get(userEmail);

                if (user == null)
        {
            return new Response()
            {
                Message = "User not found.",
                HttpStatus = (int)HttpStatusCode.NotFound
            };
        }

        
        FilterDefinition<Item> filter = Builders<Item>.Filter.Regex(item => item.Title,
            new BsonRegularExpression($"^{Regex.Escape(title)}", "i"));
        var item = await repository.Find(filter);

        if (item != null && item.Isavailable && !item.IsPurchased)
        {
            if (user.Wallet >= item.Price)
            {
                // Add the item to the inventory
                inventory.items.Add(item.Id, item);

                // Update the user's wallet
                user.Wallet -= item.Price;
                await userrepository.Update(user);

                // Mark the item as purchased
                item.IsPurchased = true;
                await repository.Update(item);

                // Remove the purchased item from the inventory
                inventory.items.Remove(item.Id);

                return new Response()
                {
                    Data = item,
                    HttpStatus = 200
                };
            }
            else
            {
                return new Response()
                {
                    Message = "Insufficient funds in the user's wallet.",
                    HttpStatus = (int)HttpStatusCode.BadRequest
                };
            }
        }
        else
        {
            return new Response()
            {
                Message = "Item not found or it is not available for purchase.",
                HttpStatus = (int)HttpStatusCode.NotFound
            };
        }
    }
    catch (Exception e)
    {
        // Log the exception
        Console.WriteLine(e.ToString());

        return new Response()
        {
            Message = e.Message,
            HttpStatus = (int)HttpStatusCode.InternalServerError
        };
        }
      }




    }
}
    
