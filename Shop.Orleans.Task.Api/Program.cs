using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;
using Utility.AspNetCore;

namespace Shop.Orleans.Tasks.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DbConfig.Flag = DbFlag.MySql;
            ServiceConfig.Flag = ServiceFlag.Consul;
            //StartHelper.Start<Startup>("Shop.Product.Api", args);
            Console.Title = "Shop.Task.Api";
            IConfiguration configuration = LogHelper.Initial();
            // StartHelper.IsNew = true;
            StartHelper.CreateHostBuilder<Startup>(configuration, args).Run();
            //CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
