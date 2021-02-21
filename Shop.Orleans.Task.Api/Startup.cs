using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Orleans;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utility;
using Utility.AspNetCore;
using Utility.AspNetCore.Extensions;

namespace Shop.Orleans.Tasks.Api
{
    public class Startup
    {
        public static IClusterClient Client { get; private set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //注册微服务 
            services.AddRegisterService(Configuration, ServiceConfig.Flag);

            Utility.AspNetCore.Extensions.ServiceCollectionExtensions.AddApiVersioning(services);

            services.AddFilter()
            //全局配置Json序列化处理 方案1
            .AddJson()
          .SetCompatibilityVersion(CompatibilityVersion.Latest);
            services.AddControllers().AddControllersAsServices();
           
           
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Shop.Orleans.Task.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
#pragma warning disable CS0618 // 类型或成员已过时
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Microsoft.Extensions.Hosting.IApplicationLifetime lifetime)
#pragma warning restore CS0618 // 类型或成员已过时
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shop.Orleans.Task.Api v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger<Startup>();
            var loggerProvider = app.ApplicationServices.GetRequiredService<ILoggerProvider>();
            Client = new ClientBuilder()
                  .UseLocalhostClustering()
                  .ConfigureLogging(builder => builder.AddProvider(loggerProvider))
                  .Build();
            StartHelper.ApplicationStarted = lifetime.ApplicationStarted;
            StartHelper.ApplicationStopped = lifetime.ApplicationStopped;
            StartHelper.ApplicationStopping = lifetime.ApplicationStopping;
            lifetime.ApplicationStarted.Register( async () => {
                var attempt = 0;
                var maxAttempts = 100;
                var delay = TimeSpan.FromSeconds(1);
                await Client.Connect(async error =>
                {
                    if (lifetime.ApplicationStarted.IsCancellationRequested)
                    {
                        return false;
                    }

                    if (++attempt < maxAttempts)
                    {
                        logger.LogWarning(error,
                            "Failed to connect to Orleans cluster on attempt {@Attempt} of {@MaxAttempts}.",
                            attempt, maxAttempts);

                        try
                        {
                             Task.Delay(delay, lifetime.ApplicationStarted).Wait();
                        }
                        catch (OperationCanceledException)
                        {
                            return false;
                        }

                        return true;
                    }
                    else
                    {
                        logger.LogError(error,
                            "Failed to connect to Orleans cluster on attempt {@Attempt} of {@MaxAttempts}.",
                            attempt, maxAttempts);

                        return false;
                    }
                });
            });

            lifetime.ApplicationStopped.Register(() => {
                try
                {
                     Client.Close().Wait();
                }
                catch (OrleansException error)
                {
                    logger.LogWarning(error, "Error while gracefully disconnecting from Orleans cluster. Will ignore and continue to shutdown.");
                }
            });
            app.UseService(Configuration, ServiceConfig.Flag);
            app.Use(env, "Shop.Task.Api");
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});
        }
    }
}
