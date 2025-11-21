using Delivery.Common;
using Delivery.Common.Services; // <-- обов'язково, щоб бачити CrudServiceAsync
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.WriteLine("=== DEMO: Delivery CRUD & Models ===\n");

        // --- 1. Створюємо колекції Truck і Package ---
        var trucks = new List<Truck>
        {
            Truck.CreateNew(),
            Truck.CreateNew()
        };

        var packages = new List<Package>
        {
            Package.CreateNew(),
            Package.CreateNew()
        };

        var orders = new List<DeliveryOrder>
        {
            DeliveryOrder.CreateNew(packages[0], trucks[0]),
            DeliveryOrder.CreateNew(packages[1], trucks[1])
        };

        // --- 2. Виводимо всю інформацію ---
        Console.WriteLine("All trucks:");
        foreach (var truck in trucks)
            truck.DisplayInfo();

        Console.WriteLine("\nAll packages:");
        foreach (var pkg in packages)
            pkg.ShowInfo();

        Console.WriteLine("\nAll delivery orders:");
        foreach (var order in orders)
            order.ShowInfo();

        // --- 3. Мін, макс, середнє по вагах пакетів ---
        var minWeight = packages.Min(p => p.WeightKg);
        var maxWeight = packages.Max(p => p.WeightKg);
        var avgWeight = packages.Average(p => p.WeightKg);
        Console.WriteLine($"\nPackage Weight: Min={minWeight:F2}, Max={maxWeight:F2}, Avg={avgWeight:F2}");

        // --- 4. Асинхронне збереження у файл через CrudServiceAsync ---
        var packageService = new CrudServiceAsync<Package>("packages.json");

        foreach (var pkg in packages)
        {
            await packageService.CreateAsync(pkg);
        }

        var saved = await packageService.SaveAsync();
        Console.WriteLine(saved
            ? "\nPackages saved to file asynchronously!"
            : "\nFailed to save packages.");
    }
}
