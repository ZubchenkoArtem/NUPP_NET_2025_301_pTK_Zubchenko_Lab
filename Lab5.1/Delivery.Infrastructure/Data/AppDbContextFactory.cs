using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Delivery.Infrastructure
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // ⚠️ Підстав свою стрічку підключення
            optionsBuilder.UseSqlite("Data Source=delivery.db");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
