using System;

namespace Delivery.Common
{
    public class Truck : Vehicle
    {
        public double FuelConsumption { get; set; }
        public int Axles { get; set; }
        public bool HasRefrigeration { get; set; }

        public Truck() : base() { }

        public static Truck CreateNew()
        {
            return new Truck
            {
                LicensePlate = $"TRK-{Guid.NewGuid().ToString().Substring(0, 5)}",
                MaxLoadKg = Random.Shared.Next(1000, 10000),
                FuelConsumption = Random.Shared.NextDouble() * 50,
                Axles = Random.Shared.Next(2, 5),
                HasRefrigeration = Random.Shared.Next(0, 2) == 1
            };
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Truck {LicensePlate}, Load: {MaxLoadKg} kg, Fuel: {FuelConsumption:F2} L, Axles: {Axles}, Refrigeration: {HasRefrigeration}");
        }
    }
}
