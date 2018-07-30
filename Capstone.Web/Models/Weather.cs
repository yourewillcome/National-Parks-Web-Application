using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models
{
    public class Weather
    {
        public bool IsFahrenheit { get; set; }
        public string ParkCode { get; set; }
        public int FiveDayForecastValue { get; set; }
        public int Low { get; set; }
        public int High { get; set; }
        public string Forecast { get; set; }
        public Dictionary<string, string> WeatherTypeAndMessage { get; private set; } = new Dictionary<string, string>()
        {
            {"rain", "Pack rain gear, and wear waterproof shoes!" },
            {"snow", "Pack snow shoes!" },
            {"thunderstorms", "Seek shelter and avoid hiking on exposed ridges!" },
            {"sunny", "Pack sunblock!" },
            {"partly cloudy", "Fin shapes in those dang ol' clouds!" },
            {"cloudy", "Rootin Tootin Shapes in those dang ol' clouds!" },
        };

        public int FahrenheitToCelsius(double num)
        {
            double celsius = 0;
            celsius = (((num - 32) * 5) / 9);
            return (int)celsius;
        }
    }
}