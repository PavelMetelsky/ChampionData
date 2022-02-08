using System;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using DebugTest.Models;

namespace DebugTest
{
    public class WeatherLoggingService: IWeatherLoggingService
    {
        private const string FILE_NAME_TEMPLATE =  "weather-{0}-{1}.json";
        private const string TimeFormat = "MMMM-dd-yyyy-H-mm-ss";
        
        private readonly DateTime UnixTimeStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        private readonly WeatherFileConfiguration _weatherFileConfiguration;

        public WeatherLoggingService(WeatherFileConfiguration weatherFileConfiguration)
        {
            _weatherFileConfiguration = weatherFileConfiguration;
        }

        public async Task WriteToFile(string location, WeatherInfo weatherInfo)
        {
            var path = string.Format(FILE_NAME_TEMPLATE, location, GetTimeForFileName());
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            
            await using var streamWriter = new StreamWriter(Path.Combine(_weatherFileConfiguration.FolderPath, path), true);
            await streamWriter.WriteLineAsync(JsonSerializer.Serialize(weatherInfo, serializeOptions));
        }
        
        public string GetTimeForFileName()
        {
            var time = DateTimeOffset.Now.ToUnixTimeMilliseconds()
                       / (_weatherFileConfiguration.TimerInterval * _weatherFileConfiguration.CountEntriesInFile)
                       * (_weatherFileConfiguration.TimerInterval * _weatherFileConfiguration.CountEntriesInFile);

            return UnixTimeStampToDateTime(time).ToString(TimeFormat, CultureInfo.InvariantCulture);
        }

        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            return UnixTimeStart.AddMilliseconds(unixTimeStamp).ToLocalTime();
        }
    }
}