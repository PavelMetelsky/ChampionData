using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DebugTest.Models;

namespace DebugTest
{
    public interface IWeatherHttpClient
    {
        Task<List<Location>> SearchLocationsByName(string query, CancellationToken cancellationToken);
        Task<List<Location>> SearchLocationsByCoordinates(string query, CancellationToken cancellationToken);
        Task<WeatherInfo> GetWeatherInfoByWoeId(int woeId,CancellationToken cancellationToken);
    }
}