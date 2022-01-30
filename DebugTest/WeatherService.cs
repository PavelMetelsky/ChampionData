using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Timers;
using DebugTest.Models;

namespace DebugTest
{
    public static class WeatherService
    {
        public static Location GetLocation()
        {
            Print("Enter location:");
            var locationQuery = Console.ReadLine();

            var content = GetDataFromWeatherByUri<List<Location>>(
                locationQuery.Contains(".")
                    ? $"{Constans.LocationByCoOrdinatesUri}{locationQuery}"
                    : $"{Constans.LocationByLocationNameUri}{locationQuery}");

            return content.First();
        }

        public static void OnTimedEvent(Object source, ElapsedEventArgs e, Location location)
        {
            var path = $"weather-{location.title}-{GetTimeForFileName()}.json";

            var weatherInfo = GetDataFromWeatherByUri<WeatherInfo>($"{Constans.LocationWeatherUri}{location.woeid}");

            using StreamWriter streamWriter = new StreamWriter(Path.Combine(Constans.FolderPath, path), true);
            streamWriter.WriteLine(JsonSerializer.Serialize(weatherInfo));
        }

        private static string GetTimeForFileName()
        {
            var time = DateTimeOffset.Now.ToUnixTimeMilliseconds()
                       / (Constans.TimerInterval * Constans.CountEntriesInFile)
                       * (Constans.TimerInterval * Constans.CountEntriesInFile);

            return UnixTimeStampToDateTime(time).ToString(Constans.TimeFormat, CultureInfo.InvariantCulture);
        }

        private static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

        private static T GetDataFromWeatherByUri<T>(string uri)
        {
            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
            var response = client.GetAsync(uri).Result;
            var streamReader = new StreamReader(response.Content.ReadAsStreamAsync().Result);

            return JsonSerializer.Deserialize<T>(streamReader.ReadToEnd());
        }

        private static void Print(string text) => Console.WriteLine(text);
    }
}