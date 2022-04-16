using DataBase_Website.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace DataBase
{
    public class Startup
    {
        public static List<Models.GuidEntity> AuthorizedGuids = new List<Models.GuidEntity>();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //Debuging Purpose for whole development procces 
            AuthorizedGuids.Add(new Models.GuidEntity { Created = System.DateTime.Today.AddYears(10), Guid = "test111" });
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<DataBase_WebsiteContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DataBase_WebsiteContext")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

    public static class Html
    {
        public static bool IsDebug(this IHtmlHelper html)
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
        public static string JoinString(string[] string_)
        {
            string c = string.Empty;
            foreach (string x in string_)
                c += x + ":";
            c.Remove(c.Length - 2, 1);
            return c;
        }
    }
}
