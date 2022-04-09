using CoreModule.Domain.Users;
using UserPortalModule.Common;

namespace UserPortalModule.System.SeedSampleData
{
    public class SeedUsers
    {
        private readonly IUserPortalModuleDbContext _dbContext;

        public SeedUsers(IUserPortalModuleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SyncAdminUser(long adminRoleId)
        {
            var adminUser = _dbContext.Users.Where(x => x.UserName == "portalAdmin").FirstOrDefault();

            if (adminUser is null)
            {
                User admin = new User("admin", "admin", "admin", "admin@userportal.com");

                admin.Id = Guid.NewGuid();

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
