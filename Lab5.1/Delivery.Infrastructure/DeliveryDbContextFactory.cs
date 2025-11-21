using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.IO;

namespace Delivery.Infrastructure.Data
{
    public class DeliveryDbContextFactory : IDesignTimeDbContextFactory<DeliveryDbContext>
    {
        public DeliveryDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DeliveryDbContext>();

            // ✅ Робимо шлях до бази локальним для Infrastructure
            var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "delivery.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");

            return new DeliveryDbContext(optionsBuilder.Options);
        }
    }
}
