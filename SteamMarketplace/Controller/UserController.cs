using IdentityModel.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SteamMarketplace.Entities.DTO;
using SteamMarketplace.Entities.Response;
using SteamMarketplace.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

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
            var response = await _loginService.Usercheck(userDto);

            if (response.HttpStatus != (int)HttpStatusCode.OK)
            {
                // User does not exist or login failed
                return new Response
                {
                    Data = null,
                    HttpStatus = response.HttpStatus,
                    Message = response.Message
                };
            }

            var claims = new[]
            {
        new Claim(ClaimTypes.Name, userDto.email),
        // Add more claims if needed
    };

            // Generate a 256-bit (32-byte) key
            var keyBytes = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(keyBytes);
            }

            var key = new SymmetricSecurityKey(keyBytes);

            var token = new JwtSecurityToken(
                "your_issuer",
                "your_audience",
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new Response
            {
                Data = tokenString,
                HttpStatus = (int)HttpStatusCode.OK
            };
        }


        // Return the error response if login is unsuccessful








        [HttpPost]
        [Route("signup")]
        public async Task<Response> signup([FromBody] UserDTO userDto)
        {


            return await _loginService.CreateUser(userDto);



        }

    }
}
