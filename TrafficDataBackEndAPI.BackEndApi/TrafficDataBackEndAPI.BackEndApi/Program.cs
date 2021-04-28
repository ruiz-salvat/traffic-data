using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TrafficDataBackEndAPI.BackEndApi.Data;

namespace TrafficDataBackEndAPI.BackEndApi
{
    public class Program
    {
       public static void Main(string[] args)
        {
            IHostBuilder hostBuilder = CreateHostBuilder(args);
            IHost host = hostBuilder.Build();

            using (var scope = host.Services.CreateScope())
            {
                DbInitializer initializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
                initializer.Initialize();
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) 
        {
            IHostBuilder hostBuilder = Host.CreateDefaultBuilder(args);
            Action<IWebHostBuilder> configure = InitializeWebHostBuilder;
            return hostBuilder.ConfigureWebHostDefaults(configure);
        }

        private static void InitializeWebHostBuilder(IWebHostBuilder webHostBuilder)
        {
            webHostBuilder.UseStartup<Startup>();
        }
    }
}
