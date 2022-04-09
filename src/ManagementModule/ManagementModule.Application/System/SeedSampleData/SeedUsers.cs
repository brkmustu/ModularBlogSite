using CoreModule.Domain.Users;
using ManagementModule.Common;

namespace ManagementModule.System.SeedSampleData
{
    public class SeedUsers
    {
        private readonly IManagementModuleDbContext _context;

        public SeedUsers(IManagementModuleDbContext context)
        {
            _context = context;
        }

        public async Task SyncAdminUser(long adminRoleId)
        {
            var adminUser = _context.Users.Where(x => x.UserName == "admin").FirstOrDefault();

            if (adminUser is null)
            {
                User admin = new User("admin", "admin", "admin", "admin@management.com");

                admin.Id = Guid.NewGuid();

                if (adminRoleId > 0)
                {
                    admin.SetRoles(new[] { adminRoleId });
                }

                _context.Users.Add(admin);

                await _context.SaveChangesAsync(cancellationToken: CancellationToken.None);
            }
        }
    }
}
