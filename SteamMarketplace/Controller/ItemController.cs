using Microsoft.AspNetCore.Mvc;
using SteamMarketplace.Entities.DTO;
using SteamMarketplace.Entities.Response;
using SteamMarketplace.Services;

namespace SteamMarketplace.Controller
{
    [ApiController]
    [Route("api/v1/ItemService")]
    public class ItemController : ControllerBase
    {
        private ItemService _itemService;

        public ItemController(ItemService itemService)
        {
            _itemService = itemService;
        }
       


        [HttpPost]
        [Route("createItem")]
        public async Task<Response> signup([FromBody] ItemDTO itemDTO)
        {


            return await _itemService.CreateItem(itemDTO);



        }
    }
}
