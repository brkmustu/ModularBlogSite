using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ManagementModule
{
    public class ManagementModuleDbContextFactory : IDesignTimeDbContextFactory<ManagementModuleDbContext>
    {
        public ManagementModuleDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<ManagementModuleDbContext>()
                .UseNpgsql(configuration.GetDefaultConnectionString());

            return new ManagementModuleDbContext(builder.Options);
        }
        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../ManagementModule.WebApi/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}

