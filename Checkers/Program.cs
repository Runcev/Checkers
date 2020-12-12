using System;
using System.Threading.Tasks;
using Checkers.Extensions;
using Checkers.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Checkers
{
    class Program
    {
        static Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();
            
            ExemplifyScoping(host.Services);

            return host.RunAsync();
        }
        
        static void ExemplifyScoping(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var test = scope.ServiceProvider.GetRequiredService<TestService>();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(services => services.RegisterServices());
    }
}