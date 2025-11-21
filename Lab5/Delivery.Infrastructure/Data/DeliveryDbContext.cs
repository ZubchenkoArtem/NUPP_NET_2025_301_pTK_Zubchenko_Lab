using Delivery.Common.Entities;
using Delivery.Infrastructure.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Infrastructure.Data
{
    public class DeliveryDbContext : IdentityDbContext<DeliveryUser>
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
            base.OnModelCreating(modelBuilder); // важливо для Identity

            modelBuilder.Entity<TruckModel>().ToTable("Trucks");
            modelBuilder.Entity<DroneModel>().ToTable("Drones");

            modelBuilder.Entity<DeliveryOrderModel>()
                .HasOne(d => d.AssignedVehicle)
                .WithMany()
                .HasForeignKey(d => d.AssignedVehicleId);
        }
    }
}
