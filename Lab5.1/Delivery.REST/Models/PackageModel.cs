// Models/PackageModel.cs
using System;

namespace Delivery.REST.Models
{
    public class PackageModel
    {
        public Guid Id { get; set; }
        public string DestinationAddress { get; set; } = string.Empty;
        public double WeightKg { get; set; }
        public bool IsFragile { get; set; }
    }
}
