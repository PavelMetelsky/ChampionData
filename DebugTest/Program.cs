using System;
using System.IO;
using System.Text.Json;
using DebugTest.Models;

namespace DebugTest
{
    class Program
    {
        private const string LocationUri = "https://www.metaweather.com/api/location/search/?query=";
        private const string LocationUri2 = "https://www.metaweather.com/api/location/";
        
        static void Main(string[] args)
        {
            Print("Enter location:");
            var location = Console.ReadLine();

            var content = GetDataFromWeatherByUri($"{LocationUri}{location}");
            var id = content.Split("woeid\":")[1].Split(",")[0];

            while (true)
            {
                var weatherInfo = JsonSerializer.Deserialize<WeatherInfo>(GetDataFromWeatherByUri($"{LocationUri2}{id}"));

                Print($"{location} max temp: {weatherInfo.consolidated_weather[0].max_temp} min temp: {weatherInfo.consolidated_weather[0].min_temp}");

                var streamWriter = new StreamWriter("weather" + location + DateTimeOffset.Now.ToUnixTimeSeconds() + ".json");
                streamWriter.Write(JsonSerializer.Serialize(weatherInfo));
                System.Threading.Thread.Sleep(2000);
            }
        }

        private static string GetDataFromWeatherByUri(string uri)
        {
            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
            var response = client.GetAsync(uri).Result;
            var streamReader = new StreamReader(response.Content.ReadAsStreamAsync().Result);

            return streamReader.ReadToEnd();
        }
        
        private static void Print(string text)
        {
            Console.WriteLine(text);
        }
    }
}
