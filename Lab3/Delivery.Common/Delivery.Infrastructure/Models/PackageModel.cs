using System;
using System.ComponentModel.DataAnnotations;

namespace Delivery.Infrastructure.Models
{
    public class PackageModel
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string DestinationAddress { get; set; } = string.Empty;
        public double WeightKg { get; set; }
        public bool IsFragile { get; set; }
    }
}
