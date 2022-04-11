using CoreModule.Domain.Permissions;
using CoreModule.Domain.Roles;
using ManagementModule.Common;

namespace ManagementModule.System.SeedSampleData;

public class SeedRoles
{
    private readonly IManagementModuleDbContext _context;

    public SeedRoles(IManagementModuleDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// admin rolü sistemde tanımlı değil ise bunu ekler ve admin rol id bilgisini döndürür
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task<long> SyncAdminRole(IEnumerable<long> permissionIds)
    {
        var adminRole = _context.Roles.Where(x => x.Name == PermissionNames.SystemAdmin).FirstOrDefault();

        long adminRoleId = 0;

        if (adminRole is null)
        {
            var newAdminRole = new Role { Name = PermissionNames.SystemAdmin, Description = "Sistemsel admin rolü tanımıdır." };

            if (permissionIds.Count() > 0)
            {
                newAdminRole.PermissionIds = permissionIds.ToArray();
            }

            _context.Roles.Add(newAdminRole);

            var saveResult = await _context.SaveChangesAsync(cancellationToken: CancellationToken.None);

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
                var savedAdminRole = _context.Roles.Where(x => x.Name == PermissionNames.SystemAdmin).FirstOrDefault();
            }
        }
        else
        {
            adminRoleId = adminRole.Id;
        }

        return adminRoleId;
    }

    public async Task SyncPortalRole()
    {
        var portalRole = _context.Roles.Where(x => x.Name == PermissionNames.Portal).FirstOrDefault();

        if (portalRole is null)
        {
            //var getUserListQuery

            _context.Roles.Add(new Role { 
                Name = PermissionNames.Portal, 
                Description = "Portal kullanıcıları için oluşturulan rol tanımıdır. Management sisteminde herhangi bir yetkisi varsayılan olarak olmayacaktır.",
                PermissionIds = new long[] { }
            });

            await _context.SaveChangesAsync(cancellationToken: CancellationToken.None);
        }
    }
}
