using Microsoft.Extensions.Options;
using ManagementModule.Common;
using CoreModule.Application;

namespace ManagementModule.System.SeedSampleData
{
    public class SampleDataSeeder
    {
        private readonly IManagementModuleDbContext _dbContext;
        private readonly SystemOptions _options;

        public SampleDataSeeder(IManagementModuleDbContext dbContext, IOptions<SystemOptions> options)
        {
            _dbContext = dbContext;
            _options = options.Value;
        }

        public async Task SeedAllAsync(CancellationToken cancellationToken)
        {
            if (_options != null && _options.SeedSampleData.HasValue && _options.SeedSampleData.Value)
            {
                /// seed sample datas
                /// 

                var permissionIds = await new SeedPermissions(_dbContext).SyncAllAsync();

                var rolesSeeder = new SeedRoles(_dbContext);

                var adminRoleId = await rolesSeeder.SyncAdminRole(permissionIds);

                await new SeedUsers(_dbContext).SyncAdminUser(adminRoleId);
            }
        }
    }
}

