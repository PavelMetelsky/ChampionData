using System;
using System.IO;
using System.Text.Json;

namespace DebugTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter location:");
            var location = Console.ReadLine();

            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

            var response = client.GetAsync("https://www.metaweather.com/api/location/search/?query=" + location).Result;

            var streamReader = new StreamReader(response.Content.ReadAsStreamAsync().Result);

            var content = streamReader.ReadToEnd();

            var id = content.Split("woeid\":")[1].Split(",")[0];


            while (true)
            {
                System.Net.Http.HttpClient client2 = new System.Net.Http.HttpClient();

                var response2 = client2.GetAsync("https://www.metaweather.com/api/location/" + id ).Result;

                var streamReader2 = new StreamReader(response2.Content.ReadAsStreamAsync().Result);

                var data = streamReader2.ReadToEnd();

                var weatherInfo = JsonSerializer.Deserialize< WeatherInfo >(data);

                Console.WriteLine(location + " max temp: " + weatherInfo.consolidated_weather[0].max_temp + " min temp: " + weatherInfo.consolidated_weather[0].min_temp);

                var streamWriter = new StreamWriter("weather" + location + DateTimeOffset.Now.ToUnixTimeSeconds() + ".json");

                streamWriter.Write(JsonSerializer.Serialize(weatherInfo));

                System.Threading.Thread.Sleep(2000);
            }
        }
    }


    public class WeatherInfo
    {
        public Consolidated_Weather[] consolidated_weather { get; set; }
        public DateTime time { get; set; }
        public DateTime sun_rise { get; set; }
        public DateTime sun_set { get; set; }
        public string timezone_name { get; set; }
        public Parent parent { get; set; }
        public Source[] sources { get; set; }
        public string title { get; set; }
        public string location_type { get; set; }
        public int woeid { get; set; }
        public string latt_long { get; set; }
        public string timezone { get; set; }
    }

    public class Parent
    {
        public string title { get; set; }
        public string location_type { get; set; }
        public int woeid { get; set; }
        public string latt_long { get; set; }
    }

    public class Consolidated_Weather
    {
        public long id { get; set; }
        public string weather_state_name { get; set; }
        public string weather_state_abbr { get; set; }
        public string wind_direction_compass { get; set; }
        public DateTime created { get; set; }
        public string applicable_date { get; set; }
        public float min_temp { get; set; }
        public float max_temp { get; set; }
        public float the_temp { get; set; }
        public float wind_speed { get; set; }
        public float wind_direction { get; set; }
        public float air_pressure { get; set; }
        public int humidity { get; set; }
        public float visibility { get; set; }
        public int predictability { get; set; }
    }

    public class Source
    {
        public string title { get; set; }
        public string slug { get; set; }
        public string url { get; set; }
        public int crawl_rate { get; set; }
    }

}
