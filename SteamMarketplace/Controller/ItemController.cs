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
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SteamMarketplace.Controller
{
    [ApiController]
    [Route("api/v1/ItemService")]
    public class ItemController : ControllerBase
    {
        private MongoRepository _mongoRepository;
        private ItemService _itemService;
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly SymmetricSecurityKey _secretkey;



        public ItemController(
            ItemService itemService,
            MongoRepository mongoRepository,
            JwtSecurityTokenHandler tokenHandler,
            SymmetricSecurityKey secretkey

            )
        {
            _itemService = itemService;
            _mongoRepository = mongoRepository;
            _tokenHandler = tokenHandler;
            _secretkey = secretkey;
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

        public string GetJwtClaim(string token, string claim)
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "User.authservice",
                ValidAudiences = new[] { "SteamMarketplace.api" },
                IssuerSigningKeys = new List<SecurityKey>
        {
            _secretkey
        }
            };

            SecurityToken validatedToken;
            var principal = _tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

            if (principal == null)
            {
                throw new Exception("JWT token validation failed");
            }

            var data = principal.Claims.FirstOrDefault(x => x.Type == claim);

            if (data == null)
            {
                throw new Exception($"Claim '{claim}' not found in the JWT token");
            }

            return data.Value;
        }





        [HttpPost]
        [Route("purchaseItem/{title}")]
        [Authorize]
        public async Task<Response> ItemPurchase(string title, [FromHeader(Name = "Authorization")] string authorization)
        {
            try
            {

                string token = authorization.Replace("Bearer ", "");

                var userEmail = GetJwtClaim(token, ClaimTypes.Email);


                var response = await _itemService.ItemPurchase(title, userEmail, authorization);

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
