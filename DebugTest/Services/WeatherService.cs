using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using DebugTest.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Timer = System.Timers.Timer;

namespace DebugTest
{
    public class WeatherService : IHostedService
    {
        private readonly IWeatherHttpClient _weatherHttpClient;
        private readonly IWeatherLoggingService _weatherLoggingService;
        private readonly WeatherFileConfiguration _weatherFileConfiguration;
        private readonly ILogger<WeatherService> _logger;

        private Timer _timer;

        public WeatherService(
            IWeatherHttpClient weatherHttpClient,
            IWeatherLoggingService weatherLoggingService,
            WeatherFileConfiguration weatherFileConfiguration,
            ILogger<WeatherService> logger)
        {
            _weatherHttpClient = weatherHttpClient;
            _weatherLoggingService = weatherLoggingService;
            _weatherFileConfiguration = weatherFileConfiguration;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Print("Enter location:");
            var locationQuery = Console.ReadLine();
            var location = await GetLocation(locationQuery, cancellationToken);

            if (location == null)
                throw new ArgumentException("Invalid location");
            
            _logger.LogInformation($"Start getting weather info about location: {location.title}");

            _timer = new Timer
            {
                Interval = _weatherFileConfiguration.TimerInterval,
                AutoReset = true,
            };
            _timer.Elapsed += async (sender, e) => await OnTimedEvent(sender, e, location, cancellationToken);
            _timer.Start();
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Stop();
            
            return Task.CompletedTask;
        }

        private async Task<Location> GetLocation(string locationQuery, CancellationToken cancellationToken)
        {
            var locationsTask = locationQuery.Contains(".")
                ? _weatherHttpClient.SearchLocationsByCoordinates(locationQuery, cancellationToken)
                : _weatherHttpClient.SearchLocationsByName(locationQuery, cancellationToken);

            return (await locationsTask)?.FirstOrDefault();
        }

        private async Task OnTimedEvent(Object source, ElapsedEventArgs e, Location location, CancellationToken cancellationToken)
        {
            try
            {
                var weatherInfo = await _weatherHttpClient.GetWeatherInfoByWoeId(location.woeid, cancellationToken);
                await _weatherLoggingService.WriteToFile(location.title, weatherInfo);

                _logger.LogInformation($"New weather info saved for: {weatherInfo.time}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"New weather info not saved");
            }
        }

        private static void Print(string text) => Console.WriteLine(text);
    }
}