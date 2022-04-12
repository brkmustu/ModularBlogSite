using CoreModule.Application.SeedSampleData;
using CoreModule.Domain.Users;
using ManagementModule.Common;

namespace ManagementModule.System.SeedSampleData
{
    public class SeedUsers
    {
        private readonly IManagementModuleDbContext _dbContext;

        public SeedUsers(IManagementModuleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SyncAdminUser(long adminRoleId)
        {
            var adminUser = _dbContext.Users.Where(x => x.UserName == "admin").FirstOrDefault();

            if (adminUser is null)
            {
                User admin = SeedingConsts.AdminUser();

                if (adminRoleId > 0)
                {
                    admin.SetRoles(new[] { adminRoleId });
                }

                _dbContext.Users.Add(admin);

                await _dbContext.SaveChangesAsync(cancellationToken: CancellationToken.None);
            }
        }
    }
}
