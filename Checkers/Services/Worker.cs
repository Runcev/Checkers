using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Client.CheckersApiClient;
using Logic.Algorithm;
using Logic.Game;
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

            var state = (await checkersApiClient.GetGameInfo()).Data;
            var game = new CheckersGame(state);
            var search = new IterativeAlphaBeta(game, state.AvailableTime);
            
            var connect = await checkersApiClient.ConnectToGame();

            userStore.Token = connect.Data.Token;
            
            while (true)
            {
                var data = (await checkersApiClient.GetGameInfo()).Data;

                if (data.IsStarted && data.WhoseTurn == connect.Data.Color)
                {
                    var move = search.MakeDecision(new MapState(data));
                    
                    await checkersApiClient.MakeMove(move.Action);
                }

                await Task.Delay(200);
            }
        }
    }
}