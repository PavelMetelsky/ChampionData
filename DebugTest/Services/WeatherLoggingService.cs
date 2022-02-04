using System;
using System.Globalization;
using System.IO;
using System.Text.Json;
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

        public void WriteToFile(string location, WeatherInfo weatherInfo)
        {
            var path = String.Format(FILE_NAME_TEMPLATE, location, GetTimeForFileName());
            
            using StreamWriter streamWriter = new StreamWriter(Path.Combine(_weatherFileConfiguration.FolderPath, path), true);
            streamWriter.WriteLine(JsonSerializer.Serialize<WeatherInfo>(weatherInfo));
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