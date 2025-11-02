using System;

namespace Delivery.Common
{
    public abstract class Vehicle
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string LicensePlate { get; set; } = string.Empty;
        public double MaxLoadKg { get; set; }

        public Vehicle() { }

        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Vehicle {LicensePlate}, MaxLoad: {MaxLoadKg} kg");
        }
    }
}
