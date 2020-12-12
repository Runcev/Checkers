using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Logic.Models;

namespace Checkers.Services
{
    public class TestService
    {
        private readonly IHttpClientFactory _httpClients;

        public TestService(IHttpClientFactory factory)
        {
            _httpClients = factory;

            var response = _httpClients.CreateClient().GetFromJsonAsync<GameInfo>("http://localhost:8081/game",
                new JsonSerializerOptions(JsonSerializerDefaults.Web)
                {
                    Converters =
                    {
                        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                    }
                }
                ).Result;

            Console.WriteLine(response);
        }
    }
}