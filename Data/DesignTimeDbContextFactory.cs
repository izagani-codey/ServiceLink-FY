using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using ServiceLink.Data;

namespace ServiceLink.Data
{
    // EF Core will discover this at design time
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Option A: read appsettings.json for the connection string
            var basePath = Directory.GetCurrentDirectory();
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var config = builder.Build();
            var connStr = config.GetConnectionString("DefaultConnection");

            // Option B (fallback): hardcode a localdb string if above returns null
            if (string.IsNullOrEmpty(connStr))
            {
                connStr = "Server=(localdb)\\MSSQLLocalDB;Database=ServiceLinkDb;Trusted_Connection=True;MultipleActiveResultSets=true";
            }

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(connStr);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
