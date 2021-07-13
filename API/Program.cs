using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //This Main() is the starting point of the application.
            CreateHostBuilder(args).Build().Run();
        }

        //The below CreateDefaultBuilder() used to create an instance of the Hostbuilder class
        //with the configurations defined in config files like appsettings.json and appsettings.env.json etc.
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //Here the Startup file is getting triggered.
                    webBuilder.UseStartup<Startup>();
                });
    }
}
