using DebugTest.Models;

namespace DebugTest
{
    public interface IWeatherLoggingService
    {
        void WriteToFile(string location, WeatherInfo weatherInfo);
    }
}