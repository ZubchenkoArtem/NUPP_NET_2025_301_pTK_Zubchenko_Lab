using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delivery.Common;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== DEMO: Delivery CRUD & Models ===\n");

        // --- 1. CRUD сервіс для Truck ---
        var truckService = new CrudService<Truck>();

        Truck t1 = new Truck { LicensePlate = "AA1234BB", MaxLoadKg = 5000 };
        Truck t2 = new Truck { LicensePlate = "CC5678DD", MaxLoadKg = 3000 };

        truckService.Create(t1);
        truckService.Create(t2);

        Console.WriteLine("All trucks in service:");
        foreach (var truck in truckService.ReadAll())
        {
            truck.DisplayInfo();
        }

        // Зміна стану холодильника через подію
        t1.OnRefrigerationStatusChanged += status => 
            Console.WriteLine($"[EVENT] Truck {t1.LicensePlate} refrigeration changed to: {status}");
        t1.ChangeRefrigerationStatus(true);

        // --- 2. CRUD сервіс для Package ---
        var packageService = new CrudService<Package>();
        Package p1 = new Package("Kyiv, Main Street 1", 20);
        Package p2 = new Package("Lviv, Freedom Ave 10", 15);

        packageService.Create(p1);
        packageService.Create(p2);

        Console.WriteLine("\nAll packages in service:");
        foreach (var package in packageService.ReadAll())
        {
            package.ShowInfo();
        }

        // --- 3. CRUD сервіс для DeliveryOrder ---
        var orderService = new CrudService<DeliveryOrder>();
        DeliveryOrder o1 = new DeliveryOrder(p1, t1);
        DeliveryOrder o2 = new DeliveryOrder(p2, t2);

        orderService.Create(o1);
        orderService.Create(o2);

        Console.WriteLine("\nAll delivery orders:");
        foreach (var order in orderService.ReadAll())
        {
            Console.WriteLine($"Order {order.Id}: Package to {order.Package.DestinationAddress}, Vehicle {order.AssignedVehicle.LicensePlate}, Status {order.Status}");
        }

        // Оновимо статус замовлення
        o1.UpdateStatus("InTransit");
        o2.UpdateStatus("Delivered");

        // --- 4. Використання методу розширення ---
        t2.PrintLicensePlate();

        // --- 5. Статичний метод ---
        Vehicle.ShowVehicleCount();

        Console.WriteLine("\n=== DEMO COMPLETE ===");
    }
}