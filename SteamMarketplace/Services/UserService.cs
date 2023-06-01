using MongoDB.Driver;
using SteamMarketplace.Entities.DTO;
using SteamMarketplace.Entities;
using SteamMarketplace.Entities.Response;
using SteamMarketplace.Repository;
using System.Net;
using System.Text.RegularExpressions;

namespace SteamMarketplace.Services
{
    public class UserService
    {
        private MongoRepository repository {  get; }

        private Response response { get; set; }
        private const int attemptLimit = 3;

        private const int hoursInDay = 24;

        public UserService(MongoRepository repository) 
            {
            
                this.repository = repository;
            }

        public async Task<Response> Usercheck(UserDTO userDTO)
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Where(User => User.Email == userDTO.email);

            var user = await repository.Find(filter);
            var loginHashing = encryption(userDTO.password);


            if (user == null)
            {

                return new Response()
                {
                    HttpStatus = 404,

                    Message = "User doesn't exist"
                };

            }
            if (user.IsBlocked)
            {
                TimeSpan timeDiffrence = user.TimeBlocked - DateTime.Now;

                if (timeDiffrence.TotalHours >= hoursInDay)
                {
                    user.IsBlocked = false;
                    await repository.Update(user);

                }
            }
            if (user.Password != loginHashing)
            {


                user.Attempt++;


                if (user.Attempt == attemptLimit)
                {
                    user.IsBlocked = true;
                    user.TimeBlocked = DateTime.Now;



                    await repository.Update(user);
                    return new Response()
                    {
                        Message = "Your account was blocked for 24 Hours"
                    };
                }
                await repository.Update(user);
                return new Response()
                {
                    HttpStatus = 403,
                    Message = "Email or password is wrong!"

                };
            }

            return new Response()
            {
                Data = new User()
                {
                   
                    Email = userDTO.email,
                    Password = userDTO.password,    
                    FirstName = userDTO.firstname,
                    LastName = userDTO.lastname,
                    Status = userDTO.status,
                    Wallet  = userDTO.wallet,
                    
                   


                },

                HttpStatus = 200,


            };


        }

        public async Task<Response> CreateUser(UserDTO userDto) //function that will let the users sign up
        {


            FilterDefinition<User> filter = Builders<User>.Filter.Where(User => User.Email == userDto.email);

            var user = await repository.Find(filter);

            if (user != null)
            {

                return new Response()
                {
                    Message = "User already exist",

                    HttpStatus = 200

                };

            }
            Dictionary<string, string> passwordRegex = new Dictionary<string, string>()
            {

                {"(.*[A-Z].*)", "Password must contain at least one uppercase characer" },
                {"(.*[a-z].*)", "Password must contain at least one lowercase characer" },
                {"(.*[0-9].*)", "Password must contain at least number" },
                {"\\b\\w{12,}", "Password must be at least 12 characters long" },
                {"(.*[!@#$%^&*()_+\\-=[\\]{};':\"\\\\|,.<>\\/?].*)", "Password must contain at least one special character"}

            };

            foreach (KeyValuePair<string, string> pair in passwordRegex)
            {

                if (!Regex.IsMatch(userDto.password, pair.Key))
                {

                    return new Response()
                    {

                        Message = pair.Value,

                        HttpStatus = (int)HttpStatusCode.OK

                    };

                }

            }


            User newuser = GetNewuser(userDto);
            await repository.create(newuser);
            return new Response()
            {
                HttpStatus = 200,

                Data = new
                {
                 
                    newuser.Email, 
                    newuser.Password,
                    newuser.FirstName,
                    newuser.LastName,
                    newuser.Wallet,
                    newuser.Status,
                    
                }


            };

        }

        private User GetNewuser(UserDTO userDto)
        {
            return new User()
            {
                Email = userDto.email,

                Password = encryption(userDto.password),

                Id = Guid.NewGuid().ToString(),

                FirstName = userDto.firstname,
                LastName = userDto.lastname,
                Wallet = userDto.wallet,
                Status = userDto.status,


                

            };
        }

        private string encryption(string password)
        {


            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                // Convert the string to a byte array first, to be processed
                byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha.ComputeHash(textBytes);

                // Convert back to a string, removing the '-' that BitConverter adds
                string hash = BitConverter
                    .ToString(hashBytes)
                    .Replace("-", String.Empty);

                return hash;
            }
        }

    }
}
