using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.IO;

namespace FleetManagement.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            // baza će se uvek tražiti u folderu gde se pokreće exe
            var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "fleet.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");


            return new AppDbContext(optionsBuilder.Options);
        }
    }
}