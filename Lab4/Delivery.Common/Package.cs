using System;

namespace Delivery.Common
{
    public class Package
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string DestinationAddress { get; set; } = "Unknown";
        public double WeightKg { get; set; }
        public bool IsFragile { get; set; }

        public Package() { }

        // ✅ Новий конструктор
        public Package(string destination, double weightKg)
        {
            DestinationAddress = destination;
            WeightKg = weightKg;
            IsFragile = Random.Shared.Next(0, 2) == 1;
        }

        public static Package CreateNew()
        {
            return new Package
            {
                DestinationAddress = $"Street {Random.Shared.Next(1, 50)}",
                WeightKg = Random.Shared.NextDouble() * 40 + 1,
                IsFragile = Random.Shared.Next(0, 2) == 1
            };
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Package {Id}, Destination: {DestinationAddress}, Weight: {WeightKg:F2} kg, Fragile: {IsFragile}");
        }
    }
}
