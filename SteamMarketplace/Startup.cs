using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SteamMarketplace.Database;
using SteamMarketplace.Entities;
using SteamMarketplace.Entities.Mapper;
using SteamMarketplace.Repository;
using SteamMarketplace.Services;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Server.IISIntegration;
using System.IdentityModel.Tokens.Jwt;

namespace SteamMarketplace
{
    public class Startup
    {
        public IConfiguration configRoot
        {
            get;
        }
        public Startup(IConfiguration configuration)
        {
            configRoot = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {

            var secretKeyBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(secretKeyBytes);
            }
            var secretKey = new SymmetricSecurityKey(secretKeyBytes);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "User.authservice",
                    ValidAudiences = new[] { "SteamMarketplace.api" },
                    IssuerSigningKey = secretKey
                };
            });





            services.AddSingleton(secretKey);
            services.AddHttpContextAccessor();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<JwtSecurityTokenHandler>();




            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            services.AddMvc();
            services.AddScoped<Context>();
            services.AddScoped<UserService>();
            services.AddScoped<MongoRepository>();
            services.AddScoped<ItemService>();
            services.AddScoped<ItemRepository>();
            services.AddScoped<UserRepository>();
            services.AddScoped<Item, Item>();
            services.AddScoped<Inventory>();

            services.AddAuthentication(IISDefaults.AuthenticationScheme);


            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ItemMapper());
            });

            IMapper mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper);
            services.AddAutoMapper(typeof(ItemMapper));
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            IdentityModelEventSource.ShowPII = true;
            // app.UseHttpsRedirection();
            //  app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            // app.MapRazorPages();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
