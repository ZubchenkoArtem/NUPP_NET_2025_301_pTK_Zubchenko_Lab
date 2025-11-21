using System;

namespace Delivery.Common
{
    public class DeliveryOrder
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Package Package { get; set; } = new();
        public Vehicle AssignedVehicle { get; set; } = new Truck();
        public string Status { get; set; } = "Pending";

        public DeliveryOrder() { }

        // ✅ Новий конструктор із параметрами
        public DeliveryOrder(Package package, Vehicle vehicle)
        {
            Package = package;
            AssignedVehicle = vehicle;
            Status = "Pending";
        }

        public static DeliveryOrder CreateNew(Package package, Vehicle vehicle)
        {
            return new DeliveryOrder(package, vehicle);
        }

        public void UpdateStatus(string newStatus)
        {
            Status = newStatus;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Order {Id}: Package to {Package.DestinationAddress}, Vehicle {AssignedVehicle.LicensePlate}, Status {Status}");
        }
    }
}
