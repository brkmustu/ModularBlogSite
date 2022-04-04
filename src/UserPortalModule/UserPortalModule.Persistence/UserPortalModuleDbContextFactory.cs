using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace UserPortalModule
{
    public class UserPortalModuleDbContextFactory : IDesignTimeDbContextFactory<UserPortalModuleDbContext>
    {
        public UserPortalModuleDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<UserPortalModuleDbContext>()
                .UseNpgsql(configuration.GetDefaultConnectionString());

            return new UserPortalModuleDbContext(builder.Options);
        }
        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../UserPortalModule.WebApi/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}

