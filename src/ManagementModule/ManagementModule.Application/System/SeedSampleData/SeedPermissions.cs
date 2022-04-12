using CoreModule.Domain.Permissions;
using ManagementModule.Common;
using ManagementModule.System.Permissions;
//using CorePermissionExtensions = CoreModule.Application.Extensions.PermissionExtensions;

namespace ManagementModule.System.SeedSampleData;

public class SeedPermissions
{
    private readonly IManagementModuleDbContext _dbContext;

    public SeedPermissions(IManagementModuleDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<List<long>> SyncAllAsync()
    {
        var allNewPermissions = PermissionExtensions.GetAuthSystemPermissions();
        //var userPortalPermissions = await CorePermissionExtensions.GetAuthSystemPermissionsAsync("userPortal");
        //allNewPermissions.AddRange(userPortalPermissions);

        var permissions = _dbContext.Permissions.ToList();

        if (permissions is null || permissions.Count == 0)
        {
            _dbContext.Permissions.AddRange(allNewPermissions);
        }
        else
        {
            var diffPermissions = permissions.Except(allNewPermissions, new PermissionNameComparer());
            if (diffPermissions.Any())
            {
                _dbContext.Permissions.AddRange(diffPermissions);
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken: CancellationToken.None);

        var currentPermissions = _dbContext.Permissions.ToList();

        var permissionIds = new List<long>();

        if (currentPermissions is not null && currentPermissions.Count > 0)
        {
            permissionIds = currentPermissions.Select(x => x.Id).ToList();
        }

        return permissionIds;
    }
}
