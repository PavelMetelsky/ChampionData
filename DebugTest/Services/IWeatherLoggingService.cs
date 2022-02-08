using System.Threading;
using System.Threading.Tasks;
using DebugTest.Models;

namespace DebugTest
{
    public interface IWeatherLoggingService
    {
        Task WriteToFile(string location, WeatherInfo weatherInfo);
    }
}