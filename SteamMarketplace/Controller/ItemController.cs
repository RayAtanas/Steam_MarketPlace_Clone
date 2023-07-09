using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SteamMarketplace.Entities.DTO;
using SteamMarketplace.Entities.Response;
using SteamMarketplace.Repository;
using SteamMarketplace.Services;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;


namespace SteamMarketplace.Controller
{
    [ApiController]
    [Route("api/v1/ItemService")]
    public class ItemController : ControllerBase
    {
        private MongoRepository _mongoRepository;
        private ItemService _itemService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private HttpContext HttpContext => _httpContextAccessor.HttpContext;

        private readonly IActionContextAccessor _actionContextAccessor;

        public ItemController(
            ItemService itemService,
            MongoRepository mongoRepository,
            IHttpContextAccessor httpContextAccessor,
            IActionContextAccessor actionContextAccessor)
        {
            _itemService = itemService;
            _mongoRepository = mongoRepository;
            _httpContextAccessor = httpContextAccessor;
            _actionContextAccessor = actionContextAccessor;
        }


        [HttpPost]
        [Route("createItem")]
        public async Task<Response> signup([FromBody] ItemDTO itemDTO)
        {


            return await _itemService.CreateItem(itemDTO);


        }

        [HttpGet]
        [Route("getItem/{ItemId}")]

        public async Task<IActionResult> getItemById(
                string ItemId
            )
        {

            var result = await _itemService.GetItemById(ItemId);
            return StatusCode(result.HttpStatus, result);

        }

        [HttpGet]
        [Route("getItemByName/{title}")]

        public async Task<IActionResult> getAppointmentByName(
               string title
           )
        {

            var result = await _itemService.FindItemByName(title);
            return StatusCode(result.HttpStatus, result);

        }

        [HttpPost]
        [Route("purchaseItem/{title}")]
        [Authorize]

        public async Task<Response> ItemPurchase(string title, [FromHeader] string authorization)
        {
            try
            {
                var userEmail = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

                var response = await _itemService.ItemPurchase(title, userEmail,  authorization,  HttpContext);

                return new Response
                {
                    Data = response.Data,
                    Message = response.Message,
                    HttpStatus = response.HttpStatus
                };
            }
            catch (Exception e)
            {
                // Log and handle the exception
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
