using System;
using System.IO;
using System.Threading.Tasks;
using Checkers.Extensions;
using Checkers.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Checkers
{
    class Program
    {
        static Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();

            return host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                // .ConfigureAppConfiguration(appConfig =>
                // {
                //     appConfig.SetBasePath(Directory.GetCurrentDirectory());
                //     appConfig.AddJsonFile("appsettings.json");
                // })
                .ConfigureServices(services => services.RegisterServices());
    }
}