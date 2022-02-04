using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using DebugTest.Configuration;
using DebugTest.Models;

namespace DebugTest
{
    public class WeatherHttpClient: IWeatherHttpClient
    {
        private readonly HttpClient _client;
        private readonly WeatherApiConfiguration _configuration;

        public WeatherHttpClient(HttpClient client, WeatherApiConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }


        public Task<List<Location>> SearchLocationsByName(string query)
        {
            return GetDataByUrl<List<Location>>($"{_configuration.SearchByName}{query}");
        }
        
        public Task<List<Location>> SearchLocationsByCoordinates(string query)
        {
            return GetDataByUrl<List<Location>>($"{_configuration.SearchByCoordinates}{query}");
        }

        public Task<WeatherInfo> GetWeatherInfoByWoeId(int woeId)
        {
            return GetDataByUrl<WeatherInfo>($"{_configuration.Weather}{woeId}");
        }
        
        private async Task<T> GetDataByUrl<T>(string url)
        {
            var response = await _client.GetAsync(url);
            var stream = await response.Content.ReadAsStreamAsync();

            return await JsonSerializer.DeserializeAsync<T>(stream);
        }
    }
}