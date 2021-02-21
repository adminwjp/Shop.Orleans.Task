using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Shop.Tasks.Grains;
using Shop.Tasks.Interfaces;
using Utility;
using Utility.Config;
using Utility.Domain.Uow;
using Utility.Ef;
using Utility.Ef.Uow;

namespace Shop.Tasks.SiloHost
{
    /// <summary>
    ///参考 Orleans 实现
    ///https://github.com/dotnet/Orleans
    ///https://dotnet.github.io/orleans/
    ///Phenix https://github.com/phenixiii/Phenix.NET7/blob/master/Phenix.Client/HttpClient.cs
    /// "System.TimeoutException
    /// </summary>
    public class Program
    {
        //public static  void Main(string[] args)
        //{
        //    ISiloHost host = new SiloHostBuilder()
        //      .UseLocalhostClustering(40001, 40000,null, "TestCluster", "TestServer")
        //       .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(TestGrain).Assembly).WithReferences())
        //     // .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(TestGrain).Assembly).AddApplicationPart(typeof(ITestGrain).Assembly))
        //      .Build();
        //     host.StartAsync();
        //    System.Console.ReadKey();
        //}

        public static Task Main(string[] args)
        {
            try
            {
                //Response did not arrive on time 
                DbConfig.Flag = DbFlag.MySql;
                return new HostBuilder()
                .UseOrleans(builder =>
                {
                    builder
                        .UseLocalhostClustering()
                        .Configure<ClusterOptions>(options =>
                        {
                            options.ClusterId = "dev";
                            options.ServiceId = "dev";
                        })
                        .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                        .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(TaskGrain).Assembly).WithReferences());
                        //.AddMemoryGrainStorage(name: "test");

                     //.AddAzureBlobGrainStorage(
                     //    name: "profileStore",
                     //    configureOptions: options =>
                     //    {
                     //        // Use JSON for serializing the state in storage
                     //        options.UseJson = true;

                     //        // Configure the storage connection key
                     //        options.ConnectionString = "DefaultEndpointsProtocol=https;AccountName=data1;AccountKey=SOMETHING1";
                     //    });

                     //.AddAdoNetGrainStorage(
                     //        name: "profileStore",
                     //    configureOptions: options =>
                     //    {
                     //        // Configure the storage connection key
                     //        options.ConnectionString = "DefaultEndpointsProtocol=https;AccountName=data1;AccountKey=SOMETHING1";
                     //    });

                     //.AddMemoryGrainStorage(
                     //        name: "profileStore",
                     //    configureOptions:(MemoryGrainStorageOptions options) =>
                     //    {

                     //    });

                 })
                .ConfigureServices(services =>
                {
                    //di 
                    string consulUrl = System.Configuration.ConfigurationManager.AppSettings["ConsulUrl"];
                    System.Console.WriteLine("get ConsulUrl value " + consulUrl);
                    if (!string.IsNullOrEmpty(consulUrl))
                    {
                        consulUrl = "http://192.168.1.3:8500/";
                    }
                    string connectionString = ConfigManager.GetByConsul($"ShopTask/{DbConfig.Flag}ConnectionString");
                    services.UseEf<TaskDbContent>(connectionString);
                    services.AddScoped<IUnitWork, EfUnitWork>(it =>
                    new EfUnitWork(it.GetService<TaskDbContent>()));

                    services.Configure<ConsoleLifetimeOptions>(options =>
                    {
                        options.SuppressStatusMessages = true;
                    });
                })
                .ConfigureLogging(builder =>
                {
                    builder.AddConsole();
                })
                
                .RunConsoleAsync();
            }
            catch (System.Exception e)
            {

                throw e;
            }
        }
    }
}
