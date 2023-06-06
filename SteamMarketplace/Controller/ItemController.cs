using Microsoft.AspNetCore.Mvc;
using SteamMarketplace.Entities.DTO;
using SteamMarketplace.Entities.Response;
using SteamMarketplace.Repository;
using SteamMarketplace.Services;

namespace SteamMarketplace.Controller
{
    [ApiController]
    [Route("api/v1/ItemService")]
    public class ItemController : ControllerBase
    {
        private MongoRepository _mongoRepository;
        private ItemService _itemService;

        public ItemController(ItemService itemService, MongoRepository mongoRepository )
        {
            _itemService = itemService;
            _mongoRepository = mongoRepository;
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
    }
}
