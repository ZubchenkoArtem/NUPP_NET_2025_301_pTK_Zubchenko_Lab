using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Delivery.Common
{
    public class Drone : Vehicle
    {
        public Drone(double maxFlightTime)
        {
            MaxFlightTimeMinutes = maxFlightTime;
            BatteryLevel = 100;
        }

        public double MaxFlightTimeMinutes { get; set; }
        public double BatteryLevel { get; set; } // %
        public double MaxAltitudeMeters { get; set; }

        // метод
        public void Fly(double minutes)
        {
            BatteryLevel -= minutes / MaxFlightTimeMinutes * 100;
            Console.WriteLine($"Drone flew {minutes} min, battery now {BatteryLevel}%");
        }
    }
}
