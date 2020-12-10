using System.Threading.Tasks;
using Checkers.Extensions;
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
                .ConfigureServices(services => services.RegisterServices());
    }
}