using CoreModule.Application;
using Microsoft.Extensions.Options;
using UserPortalModule.Common;

namespace UserPortalModule.System.SeedSampleData
{
    public class SampleDataSeeder
    {
        private readonly IUserPortalModuleDbContext _dbContext;
        private readonly SystemOptions _options;

        public SampleDataSeeder(IUserPortalModuleDbContext dbContext, IOptions<SystemOptions> options)
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

                var adminRoleId = await new SeedRoles(_dbContext).SyncAdminRole(permissionIds);

                await new SeedUsers(_dbContext).SyncAdminUser(adminRoleId);
            }
        }
    }
}

