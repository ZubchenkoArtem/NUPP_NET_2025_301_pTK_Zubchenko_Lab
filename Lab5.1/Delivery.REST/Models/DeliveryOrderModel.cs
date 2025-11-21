// Models/DeliveryOrderModel.cs
using System;

namespace Delivery.REST.Models
{
    public class DeliveryOrderModel
    {
        public Guid Id { get; set; }
        public Guid PackageId { get; set; }
        public Guid VehicleId { get; set; }
        public string Status { get; set; } = "Pending";
    }
}
