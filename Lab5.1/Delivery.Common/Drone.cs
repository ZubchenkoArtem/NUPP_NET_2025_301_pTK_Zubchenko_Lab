using System;

namespace Delivery.Common
{
    public class Drone : Vehicle
    {
        public double MaxFlightTimeMinutes { get; set; }
        public double MaxAltitudeMeters { get; set; }
        public double BatteryLevel { get; set; }

        public Drone() : base() { }

        public static Drone CreateNew()
        {
            return new Drone
            {
                LicensePlate = $"DRN-{Guid.NewGuid().ToString().Substring(0, 5)}",
                MaxLoadKg = Random.Shared.Next(1, 50),
                MaxFlightTimeMinutes = Random.Shared.Next(10, 120),
                MaxAltitudeMeters = Random.Shared.Next(50, 500),
                BatteryLevel = Random.Shared.NextDouble() * 100
            };
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Drone {LicensePlate}, Load: {MaxLoadKg} kg, FlightTime: {MaxFlightTimeMinutes} min, Altitude: {MaxAltitudeMeters} m, Battery: {BatteryLevel:F1}%");
        }
    }
}
