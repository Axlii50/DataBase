using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DataBase
{
    public class Program
    {
        public static void Main(string[] args)
        {



            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        /// <summary>
        /// make sure that all neccessary folders exist 
        /// </summary>
        public static void SetupEnvironment()
        {
            //images
            string ImagePath = GetPath("Images");
            if (System.IO.File.Exists(ImagePath)) System.IO.File.Create(ImagePath);
        }

        /// <summary>
        /// Return given path with full as full path not as part
        /// </summary>
        /// <param name="PathAdd"></param>
        /// <returns></returns>
        public static string GetPath(string PathAdd)
        {
            string AppFullname = Assembly.GetEntryAssembly().FullName;
            string Appname = AppFullname.Split(',')[0] + ".exe";
            string Path = Assembly.GetEntryAssembly().Location.Replace(Appname, "");
            return Path + @"\" + PathAdd;
        }
    }
}
