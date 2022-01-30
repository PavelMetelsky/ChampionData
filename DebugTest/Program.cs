using System;
using System.Timers;

namespace DebugTest
{
    public static class Program
    {
        private static Timer _timer;

        public static void Main()
        {
            try
            {
                var location = WeatherService.GetLocation();

                _timer = new Timer {Interval = Constans.TimerInterval};
                _timer.Elapsed += (sender, e) => WeatherService.OnTimedEvent(sender, e, location);
                _timer.AutoReset = true;
                _timer.Enabled = true;

                Console.WriteLine("Press the Enter key to exit the program at any time... ");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}