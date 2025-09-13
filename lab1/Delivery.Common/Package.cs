using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery.Common
{
    public class Package
    {
        public Package(string address, double weight)
        {
            Id = Guid.NewGuid();
            DestinationAddress = address;
            WeightKg = weight;
        }

        public Guid Id { get; set; }
        public double WeightKg { get; set; }
        public string DestinationAddress { get; set; }
        public bool IsFragile { get; set; }

        // метод
        public void ShowInfo()
        {
            Console.WriteLine($"Package {Id}, Weight: {WeightKg}kg, Destination: {DestinationAddress}");
        }
    }
}