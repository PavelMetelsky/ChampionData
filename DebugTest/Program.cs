using System.Threading.Tasks;
using DebugTest.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DebugTest
{
    public static class Program
    {
        public static async Task Main()
        {
            await new HostBuilder()
                .ConfigureAppConfiguration((builderContext, config) => { config.AddJsonFile("appsettings.json"); })
                .ConfigureServices((hostContext, services) =>
                {
                    var weatherApiConfig = hostContext.Configuration.GetSection("WeatherApi")
                        .Get<WeatherApiConfiguration>();
                    services.AddSingleton(weatherApiConfig);
                    var weatherFileConfig = hostContext.Configuration.GetSection("WeatherFile")
                        .Get<WeatherFileConfiguration>();
                    
                    services.AddSingleton(weatherFileConfig);

                    services.AddHttpClient();
                    services.AddLogging(configure => configure.AddConsole());
                    services.AddTransient<IWeatherHttpClient, WeatherHttpClient>();
                    services.AddTransient<IWeatherLoggingService, WeatherLoggingService>();
                    services.AddHostedService<WeatherService>();
                })
                .RunConsoleAsync();
        }
    }
}