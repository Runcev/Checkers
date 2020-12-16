using System.Net.Http.Headers;
using Checkers.Services;
using Client.CheckersApiClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Services;

namespace Checkers.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddScoped<UserStore>();
            services.AddScoped<ApiClient>();
            services.AddScoped<CheckersApiClient>();
   
            services.AddHostedService<Worker>();
        }
    }
}