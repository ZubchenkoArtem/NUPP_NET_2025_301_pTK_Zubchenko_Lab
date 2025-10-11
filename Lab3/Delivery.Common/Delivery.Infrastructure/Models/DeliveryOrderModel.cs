using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Delivery.Infrastructure.Models
{
    public class DeliveryOrderModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        // FK до пакета
        public Guid PackageId { get; set; }
        public PackageModel? Package { get; set; }

        // FK до транспортного засобу (TruckModel або DroneModel) — зберігається Id
        public Guid AssignedVehicleId { get; set; }   // <-- використай цю назву у мапперах
        public VehicleModel? AssignedVehicle { get; set; }

        public string Status { get; set; } = "Pending";
        public DateTime ScheduledDate { get; set; } = DateTime.UtcNow;
    }
}
