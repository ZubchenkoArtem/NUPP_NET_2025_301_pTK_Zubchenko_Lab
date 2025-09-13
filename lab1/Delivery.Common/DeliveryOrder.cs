using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery.Common
{
    public class DeliveryOrder
    {
        public DeliveryOrder(Package package, Vehicle vehicle)
        {
            Id = Guid.NewGuid();
            Package = package;
            AssignedVehicle = vehicle;
            ScheduledDate = DateTime.Now;
            Status = "Pending";
        }

        public Guid Id { get; set; }
        public Package Package { get; set; }
        public Vehicle AssignedVehicle { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string Status { get; set; }

        // метод
        public void UpdateStatus(string status)
        {
            Status = status;
            Console.WriteLine($"Order {Id} status updated to {status}");
        }
    }
}