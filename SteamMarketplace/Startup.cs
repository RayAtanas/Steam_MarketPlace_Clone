using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SteamMarketplace.Database;
using SteamMarketplace.Entities;
using SteamMarketplace.Entities.Mapper;
using SteamMarketplace.Repository;
using SteamMarketplace.Services;
using System.Text;

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
                    ValidIssuer = "your_issuer",
                    ValidAudience = "your_audience",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key"))
                };
            });

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            services.AddMvc();
            services.AddScoped<Context> ();
            services.AddScoped<UserService>();
            services.AddScoped<MongoRepository>();
            services.AddScoped<ItemService>();
            services.AddScoped<ItemRepository>();
            services.AddScoped<UserRepository>();
            services.AddScoped<Item, Item>();
          
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

            // app.UseHttpsRedirection();
            //  app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            // app.MapRazorPages();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
