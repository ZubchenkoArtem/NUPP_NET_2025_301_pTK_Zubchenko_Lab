// Models/TruckModel.cs
using System;

namespace Delivery.REST.Models
{
    public class TruckModel
    {
        public Guid Id { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
        public double MaxLoadKg { get; set; }
        public bool HasRefrigeration { get; set; }
    }
}
