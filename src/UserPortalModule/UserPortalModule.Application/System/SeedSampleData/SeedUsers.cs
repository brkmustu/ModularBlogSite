using CoreModule.Application.Extensions.Hashing;
using CoreModule.Domain.Users;
using UserPortalModule.Common;

namespace UserPortalModule.System.SeedSampleData
{
    public class SeedUsers
    {
        private const string AdminPassword = "1qaz!2wsx";
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

                var encryptedPassword = AdminPassword.CreatePasswordHash();

                admin.SetPasswordHash(encryptedPassword.PasswordHash);
                admin.SetPasswordSalt(encryptedPassword.PasswordSalt);

                admin.Id = Guid.NewGuid();

                admin.SetUserStatus(UserStatusType.Active);
                admin.Activate();

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
