using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace TWMSServer
{
    public class TWMSServerContextFactory : IDesignTimeDbContextFactory<TWMSServerContext>
    {
        public TWMSServerContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json") // Make sure this file exists
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<TWMSServerContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("TWMSServerConnectionString"));

            return new TWMSServerContext(optionsBuilder.Options);
        }
    }
}
