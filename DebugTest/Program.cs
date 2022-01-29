using System;
using System.IO;
using System.Text.Json;
using DebugTest.Models;

namespace DebugTest
{
    class Program
    {
        private const string LocationUriByLocationName = "https://www.metaweather.com/api/location/search/?query=";
        private const string LocationUriByCoOrdinates = "https://www.metaweather.com/api/location/search/?lattlong=";
        private const string LocationUri2 = "https://www.metaweather.com/api/location/";
        
        static void Main(string[] args)
        {
            var location = "minsk"; // GetLocation()
            
            Print("Enter location:");
            location = Console.ReadLine();

            var content = location.Contains(".")
                ? GetDataFromWeatherByUri($"{LocationUriByCoOrdinates}{location}")
                : GetDataFromWeatherByUri($"{LocationUriByLocationName}{location}");
            
            var id = content.Split("woeid\":")[1].Split(",")[0];

            while (true)
            {
                var weatherInfo = JsonSerializer.Deserialize<WeatherInfo>(GetDataFromWeatherByUri($"{LocationUri2}{id}"));

                Print($"{location} max temp: {weatherInfo.consolidated_weather[0].max_temp} min temp: {weatherInfo.consolidated_weather[0].min_temp}");

                //{DateTimeOffset.Now.ToUnixTimeSeconds()}
                var path = $"weather-{location}.json";
                string docPath = "C:\\Users\\Admah\\Documents\\TestProject\\ChampionData\\DebugTest\\Results";
                
                using (StreamWriter streamWriter =  new StreamWriter(Path.Combine(docPath, path), true))
                {
                    streamWriter.WriteLine(JsonSerializer.Serialize(weatherInfo));
                }
                
                System.Threading.Thread.Sleep(2000);
            }
        }

        private static string GetLocation()
        {
            Print("Enter location:");
            
            return Console.ReadLine();
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
