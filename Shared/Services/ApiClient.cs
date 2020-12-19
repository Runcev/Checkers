using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Shared.Services
{
    public class ApiClient
    {
        public static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
        {
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
        };

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserStore _userStore;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ApiClient> _logger;

        public ApiClient(IHttpClientFactory httpClientFactory, UserStore userStore, IConfiguration configuration, ILogger<ApiClient> logger)
        {
            _httpClientFactory = httpClientFactory;
            _userStore = userStore;
            _configuration = configuration;
            _logger = logger;
        }

        private string AddBasePath(string path) => $"{_configuration["BaseApiUri"]}{path}";
        
        public Task<T> Get<T>(string url) => SendRequest(client => client.GetFromJsonAsync<T>(AddBasePath(url), JsonOptions));

        public Task<HttpResponseMessage> Post(string url, object body) =>
            SendRequest(client => client.PostAsJsonAsync(AddBasePath(url), body, JsonOptions));

        private async Task<T> SendRequest<T>(Func<HttpClient, Task<T>> makeRequest)
        {
            using var client = _httpClientFactory.CreateClient();

            if (_userStore.Token != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", _userStore.Token);
            }

            return await makeRequest(client);
        }
    }
}