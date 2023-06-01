using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SteamMarketplace.Entities.DTO;
using SteamMarketplace.Entities.Response;
using SteamMarketplace.Services;

namespace SteamMarketplace.Controller
{
    [ApiController]
    [Route("api/v1/loginService")]
    public class UserController : ControllerBase
    {
        private UserService _loginService;

        public UserController(UserService loginService)
        {
            _loginService = loginService;
        }
        /* [HttpGet("{id}")]

         public async Task<ActionResult> GetUsers(string id) 
         {

         }
        */

        [HttpPost]
        [Route("login")]
        public async Task<Response> login([FromBody] UserDTO userDto)
        {


            return await _loginService.Usercheck(userDto);



        }


        [HttpPost]
        [Route("signup")]
        public async Task<Response> signup([FromBody] UserDTO userDto)
        {


            return await _loginService.CreateUser(userDto);



        }
    }
}
