using Microsoft.Extensions.DependencyInjection;

namespace Checkers.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddHttpClient();
        }
    }
}