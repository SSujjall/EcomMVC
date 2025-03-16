using EcomSiteMVC.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EcomSiteMVC.Infrastructure.Data
{
    public class DeployTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // Set up configuration to read the connection string from environment variables
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables() // This will allow reading from environment variables
                .Build();

            // Retrieve the connection string from environment variables or appsettings.json
            var connectionString = configuration.GetConnectionString("EcomDB")
                                   ?? Environment.GetEnvironmentVariable("ConnectionStrings__EcomDB"); // This is variable from yml file env

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'EcomDB' not found.");
            }

            optionsBuilder.UseSqlServer(connectionString); // Or use the appropriate provider

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
