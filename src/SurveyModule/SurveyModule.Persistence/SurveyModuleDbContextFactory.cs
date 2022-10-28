using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SurveyModule
{
    public class SurveyModuleDbContextFactory : IDesignTimeDbContextFactory<SurveyModuleDbContext>
    {
        public SurveyModuleDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<SurveyModuleDbContext>()
                .UseNpgsql(configuration.GetDefaultConnectionString());

            return new SurveyModuleDbContext(builder.Options);
        }
        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../SurveyModule.WebApi/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}

