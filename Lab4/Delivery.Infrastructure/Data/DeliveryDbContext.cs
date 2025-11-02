using Microsoft.EntityFrameworkCore;
using Delivery.Infrastructure.Models;

namespace Delivery.Infrastructure.Data
{
    public class DeliveryDbContext : DbContext
    {
        public DeliveryDbContext(DbContextOptions<DeliveryDbContext> options)
            : base(options)
        {
        }

        public DbSet<TruckModel> Trucks { get; set; } = null!;
        public DbSet<DroneModel> Drones { get; set; } = null!;
        public DbSet<VehicleModel> Vehicles { get; set; } = null!;
        public DbSet<PackageModel> Packages { get; set; } = null!;
        public DbSet<DeliveryOrderModel> DeliveryOrders { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Наслідування TPT
            modelBuilder.Entity<TruckModel>().ToTable("Trucks");
            modelBuilder.Entity<DroneModel>().ToTable("Drones");

            // ❌ Прибираємо зв’язок WithMany(v => v.DeliveryOrders),
            // бо VehicleModel не має цієї властивості.
            modelBuilder.Entity<DeliveryOrderModel>()
                .HasOne(d => d.AssignedVehicle)
                .WithMany() // просто вкажемо без зворотного зв’язку
                .HasForeignKey(d => d.AssignedVehicleId);
        }
    }
}
