using System;
using System.Threading;
using System.Threading.Tasks;
using Client.CheckersApiClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.Models;
using Shared.Services;

namespace Checkers.Services
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _services;

        public Worker(ILogger<Worker> logger, IServiceProvider services)
        {
            _logger = logger;
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _services.CreateScope();

            var userStore = scope.ServiceProvider.GetRequiredService<UserStore>();
            var checkersApiClient = scope.ServiceProvider.GetRequiredService<CheckersApiClient>();
            
            _logger.LogInformation((await checkersApiClient.GetGameInfo()).ToString());
            
            var con = await checkersApiClient.ConnectToGame();

            userStore.Token = con.Data.Token;

            while (!stoppingToken.IsCancellationRequested)
            {
                if ((await checkersApiClient.GetGameInfo()).Data.WhoseTurn == con.Data.Color)
                {
                    
                }
            }
        }
    }
}