using System;

namespace Delivery.Common
{
    public class Package
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string DestinationAddress { get; set; } = string.Empty;
        public double WeightKg { get; set; }
        public bool IsFragile { get; set; }

        public static Package CreateNew()
        {
            return new Package
            {
                DestinationAddress = $"Street {Random.Shared.Next(1, 100)}",
                WeightKg = Random.Shared.NextDouble() * 50,
                IsFragile = Random.Shared.Next(0, 2) == 1
            };
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Package {Id}, Destination: {DestinationAddress}, Weight: {WeightKg:F2} kg, Fragile: {IsFragile}");
        }
    }
}
