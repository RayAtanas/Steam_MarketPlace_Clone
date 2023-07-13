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



namespace SteamMarketplace.Controller
{
    [ApiController]
    [Route("api/v1/loginService")]
    public class UserController : ControllerBase
    {
        private readonly SymmetricSecurityKey _secretkey;
        private UserService _loginService;

        public UserController(UserService loginService,SymmetricSecurityKey secretkey)
        {
            _loginService = loginService;
            _secretkey = secretkey;
        }
        

        [HttpPost]
        [Route("login")]

        public async Task<Response> Login([FromBody] UserDTO userDto)
        {
            var response = await _loginService.Usercheck(userDto);

            if (response.HttpStatus != (int)HttpStatusCode.OK)
            {
                return new Response
                {
                    HttpStatus = response.HttpStatus,
                    Message = response.Message
                };
            }

          var claims = new[]
{
    new Claim(ClaimTypes.NameIdentifier, userDto.email), 
    new Claim(ClaimTypes.Name, userDto.email),
};


            var signingCredentials = new SigningCredentials(_secretkey, SecurityAlgorithms.HmacSha256);

            var header = new JwtHeader(signingCredentials);
            var payload = new JwtPayload
    {
        { "iss", "User.authservice" },
        { "aud", "SteamMarketplace.api" },
        { "email", userDto.email },
        { "exp", DateTimeOffset.UtcNow.AddDays(1).ToUnixTimeSeconds() }
    };

            var jwtToken = new JwtSecurityToken(header, payload);

            var jwtHandler = new JwtSecurityTokenHandler();
            var tokenString = jwtHandler.WriteToken(jwtToken);

            return new Response
            {
                Data = tokenString,
                HttpStatus = (int)HttpStatusCode.OK
            };
        }



        [HttpPost]
        [Route("signup")]
        public async Task<Response> signup([FromBody] UserDTO userDto)
        {


            return await _loginService.CreateUser(userDto);



        }

    }
}
