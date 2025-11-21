using Delivery.Common;
using Delivery.Common.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Infrastructure
{
    public class AppDbContext : IdentityDbContext<DeliveryUser>
    {
        public DbSet<Package> Packages => Set<Package>();
        public DbSet<Truck> Trucks => Set<Truck>();
        public DbSet<DeliveryOrder> Orders => Set<DeliveryOrder>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
