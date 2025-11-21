using System;

namespace Delivery.Infrastructure.Models
{
    public class DroneModel : VehicleModel
    {
        // Специфічні для дрона
        public double BatteryLevel { get; set; }
        public double MaxPayloadKg { get; set; }    // відповідає доменній Drone.MaxPayloadKg
        public double MaxFlightTimeMinutes { get; set; }
        public double MaxAltitudeMeters { get; set; }
    }
}
