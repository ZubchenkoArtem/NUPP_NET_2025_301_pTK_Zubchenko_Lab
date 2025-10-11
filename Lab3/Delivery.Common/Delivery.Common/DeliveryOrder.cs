using Delivery.Common;

public class DeliveryOrder
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Package Package { get; set; }
    public Truck AssignedVehicle { get; set; }
    public string Status { get; set; } = "Pending";

    public static DeliveryOrder CreateNew(Package pkg, Truck truck)
    {
        return new DeliveryOrder
        {
            Package = pkg,
            AssignedVehicle = truck
        };
    }

    public void ShowInfo()
    {
        Console.WriteLine($"Order {Id}: Package to {Package.DestinationAddress}, Vehicle {AssignedVehicle.LicensePlate}, Status {Status}");
    }

    public void UpdateStatus(string status) => Status = status;
}
