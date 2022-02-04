using System.Collections.Generic;
using System.Threading.Tasks;
using DebugTest.Models;

namespace DebugTest
{
    public interface IWeatherHttpClient
    {
        Task<List<Location>> SearchLocationsByName(string query);
        Task<List<Location>> SearchLocationsByCoordinates(string query);
        Task<WeatherInfo> GetWeatherInfoByWoeId(int woeId);
    }
}