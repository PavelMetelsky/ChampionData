using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DebugTest.Configuration;
using DebugTest.Models;

namespace DebugTest
{
    public class WeatherHttpClient : IWeatherHttpClient
    {
        private readonly HttpClient _client;
        private readonly WeatherApiConfiguration _configuration;

        public WeatherHttpClient(HttpClient client, WeatherApiConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }

        public Task<List<Location>> SearchLocationsByName(string query, CancellationToken cancellationToken)
        {
            return GetDataByUrl<List<Location>>($"{_configuration.SearchByName}{query}", cancellationToken);
        }

        public Task<List<Location>> SearchLocationsByCoordinates(string query, CancellationToken cancellationToken)
        {
            return GetDataByUrl<List<Location>>($"{_configuration.SearchByCoordinates}{query}", cancellationToken);
        }

        public Task<WeatherInfo> GetWeatherInfoByWoeId(int woeId, CancellationToken cancellationToken)
        {
            return GetDataByUrl<WeatherInfo>($"{_configuration.Weather}{woeId}", cancellationToken);
        }

        private async Task<T> GetDataByUrl<T>(string url, CancellationToken cancellationToken)
        {
            var response = await _client.GetAsync(url, cancellationToken);
            var stream = await response.Content.ReadAsStreamAsync();

            return await JsonSerializer.DeserializeAsync<T>(stream, cancellationToken: cancellationToken);
        }
    }
}