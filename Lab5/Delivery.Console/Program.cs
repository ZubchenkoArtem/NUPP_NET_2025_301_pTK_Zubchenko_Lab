using Microsoft.EntityFrameworkCore;
using Delivery.Infrastructure.Data; // тут наш однозначний контекст
using Delivery.Common;
using Delivery.Common.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    private static readonly object _lockObj = new();
    private static readonly SemaphoreSlim _semaphore = new(3);
    private static readonly AutoResetEvent _autoEvent = new(false);
    private static readonly ThreadLocal<Random> _random = new(() => new Random());

    static async Task Main()
    {
        Console.WriteLine("=== Delivery Console Async CRUD Demo ===");

        // =======================
        // 1️⃣ Налаштування DbContext і міграції
        // =======================
        var builder = new DbContextOptionsBuilder<DeliveryDbContext>();
        builder.UseSqlite("Data Source=delivery.db");

        using var db = new DeliveryDbContext(builder.Options);
        await db.Database.MigrateAsync();

        // =======================
        // 2️⃣ Підготовка шляху для збереження JSON
        // =======================
        var filePath = "output/data.json";
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

        // =======================
        // 3️⃣ Створення CRUD сервісу для Package
        // =======================
        var packageService = new CrudServiceAsync<Package>(filePath);

        int total = 10000;
        int degreeOfParallelism = Environment.ProcessorCount * 2;
        Console.WriteLine($"Creating {total} packages in parallel (degree {degreeOfParallelism})...");

        var sw = Stopwatch.StartNew();
        var createdIds = new ConcurrentBag<Guid>();

        var range = Partitioner.Create(0, total);
        Parallel.ForEach(range, new ParallelOptions { MaxDegreeOfParallelism = degreeOfParallelism }, (chunk) =>
        {
            for (int i = chunk.Item1; i < chunk.Item2; i++)
            {
                var pkg = Package.CreateNew();

                lock (_lockObj) { }

                _semaphore.Wait();
                try
                {
                    var t = packageService.CreateAsync(pkg);
                    t.Wait();
                    if (t.Result)
                        createdIds.Add(pkg.Id);
                }
                finally
                {
                    _semaphore.Release();
                }
            }
        });

        sw.Stop();
        Console.WriteLine($"Finished creation in {sw.ElapsedMilliseconds} ms. Created count: {createdIds.Count}");

        _autoEvent.Set();

        Console.WriteLine("Saving collection to file asynchronously...");
        var saveOk = await packageService.SaveAsync();
        Console.WriteLine($"Save result: {saveOk}");

        var all = (await packageService.ReadAllAsync()).ToArray();
        if (all.Length > 0)
        {
            var weights = all.Select(p => p.WeightKg).ToArray();
            var min = weights.Min();
            var max = weights.Max();
            var avg = weights.Average();
            Console.WriteLine($"\nPackages stats: Count={all.Length}, WeightKg => min={min:F2}, max={max:F2}, avg={avg:F2}");
        }

        Console.WriteLine("\nPagination demo (page 1, amount 5):");
        var page1 = await packageService.ReadAllAsync(1, 5);
        foreach (var p in page1) Console.WriteLine($"{p.Id} -> {p.WeightKg} kg to {p.DestinationAddress}");

        Console.WriteLine("\nPagination demo (page 2, amount 5):");
        var page2 = await packageService.ReadAllAsync(2, 5);
        foreach (var p in page2) Console.WriteLine($"{p.Id} -> {p.WeightKg} kg to {p.DestinationAddress}");

        await DemoSyncPrimitives();

        Console.WriteLine("\nAll done. Press any key to exit.");
        Console.ReadKey();
    }

    private static async Task DemoSyncPrimitives()
    {
        Console.WriteLine("\n--- Demo of sync primitives ---");

        int counter = 0;
        var tasksLock = Enumerable.Range(0, 50).Select(async _ =>
        {
            await Task.Delay(_random.Value.Next(1, 10));
            lock (_lockObj) counter++;
        }).ToArray();
        await Task.WhenAll(tasksLock);
        Console.WriteLine($"Lock demo result: counter={counter} (expected 50)");

        var sem = new SemaphoreSlim(2);
        var tasksSem = Enumerable.Range(0, 10).Select(async i =>
        {
            await sem.WaitAsync();
            try
            {
                Console.WriteLine($"Semaphore task {i} entered");
                await Task.Delay(100);
            }
            finally { sem.Release(); }
        }).ToArray();
        await Task.WhenAll(tasksSem);
        Console.WriteLine("SemaphoreSlim demo done");

        var auto = new AutoResetEvent(false);
        var waiter = Task.Run(() =>
        {
            Console.WriteLine("AutoResetEvent waiter waiting...");
            auto.WaitOne();
            Console.WriteLine("AutoResetEvent waiter received signal!");
        });
        await Task.Delay(300);
        auto.Set();
        await waiter;

        Console.WriteLine("--- sync primitives demo finished ---");
    }
}
