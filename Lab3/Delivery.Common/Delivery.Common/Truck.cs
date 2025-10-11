using System;

namespace Delivery.Common
{
    public class Truck : Vehicle
    {
        public double FuelConsumption { get; set; }
        public bool HasRefrigeration { get; set; }
        public int NumberOfAxles { get; set; }

        public Truck() : base() { }

        public static Truck CreateNew()
        {
            return new Truck
            {
                LicensePlate = $"TRK-{Guid.NewGuid().ToString().Substring(0, 5)}",
                MaxLoadKg = Random.Shared.Next(1000, 10000),
                FuelConsumption = Random.Shared.NextDouble() * 50,
                HasRefrigeration = Random.Shared.Next(0, 2) == 1,
                NumberOfAxles = Random.Shared.Next(2, 6)
            };
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Truck {LicensePlate}, Load: {MaxLoadKg} kg, Fuel: {FuelConsumption:F2} L, Axles: {NumberOfAxles}, Refrigeration: {HasRefrigeration}");
        }
    }
}
