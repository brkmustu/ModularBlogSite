using CoreModule.Domain.Roles;
using UserPortalModule.Common;

namespace UserPortalModule.System.SeedSampleData;

public class SeedRoles
{
    private readonly IUserPortalModuleDbContext _dbContext;

    public SeedRoles(IUserPortalModuleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// admin rolü sistemde tanımlı değil ise bunu ekler ve admin rol id bilgisini döndürür
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task<long> SyncAdminRole(IEnumerable<long> permissionIds)
    {
        var adminRole = _dbContext.Roles.Where(x => x.Name == "Admin").FirstOrDefault();

        long adminRoleId = 0;

        if (adminRole is null)
        {
            var newAdminRole = new Role { Name = "Admin", Description = "Portal uygulaması için oluşturulan admin rolü tanımıdır. Portal modülünde tam yetkiye sahip olacaktır." };

            newAdminRole.PermissionIds = permissionIds.ToArray();

            _dbContext.Roles.Add(newAdminRole);

            var saveResult = await _dbContext.SaveChangesAsync(cancellationToken: CancellationToken.None);

            if (saveResult != 1)
            {
                throw new SystemException("Veri tabanına temel kayıtların yüklenmesi esnasında hala oluştu!");
            }

            if (newAdminRole.Id > 0)
            {
                adminRoleId = newAdminRole.Id;
            }
            else
            {
                var savedAdminRole = _dbContext.Roles.Where(x => x.Name == "Admin").FirstOrDefault();
            }
        }
        else
        {
            adminRoleId = adminRole.Id;
        }

        return adminRoleId;
    }
}
