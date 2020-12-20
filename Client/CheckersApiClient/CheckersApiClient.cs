using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
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
        public async Task<Connect> ConnectToGame() => 
            await (await _apiClient.Post($"/game?team_name=PepeLaugh", null))
            .Content.ReadFromJsonAsync<Connect>(ApiClient.JsonOptions);
        public Task<HttpResponseMessage> MakeMove((int from, int to) moveP)
        {
            var move = new[] {moveP.from, moveP.to};
            var moveR = new {move};
            return _apiClient.Post("/move", moveR);
        }
    }
}