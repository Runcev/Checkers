using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Shared.Models;
using Shared.Services;

namespace Client.CheckersApiClient
{
    public class CheckersApiClient
    {
        private readonly ApiClient _apiClient;

        public CheckersApiClient(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public Task<GameInfo> GetGameInfo() => _apiClient.Get<GameInfo>("/game");
        public Task ConnectToGame() => _apiClient.Post("/game?team_name=PepeLaugh", null);
    }
}