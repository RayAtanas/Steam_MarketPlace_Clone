using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using SteamMarketplace.Database;
using SteamMarketplace.Entities;
using SteamMarketplace.Entities.Mapper;
using SteamMarketplace.Repository;
using SteamMarketplace.Services;

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
            services.AddScoped<IMongoRepository, MongoRepository>();
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ItemMapper());
            });

            IMapper mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper);
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
