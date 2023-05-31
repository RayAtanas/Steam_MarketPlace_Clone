using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

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
