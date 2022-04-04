using ManagementModule.Common;
using CoreModule.Domain.Permissions;
using System.Diagnostics.CodeAnalysis;

namespace ManagementModule.System.Permissions;

public class SyncPermissions
{
    public async Task SyncAllAsync(IManagementModuleDbContext dbContext)
    {
        var commandTypes = ApplicationLayerBootstrapper.GetCommandTypes();
        var queryTypes = ApplicationLayerBootstrapper.GetQueryTypes();

        var commandPermissions = commandTypes.Select(x => new Permission { Name = x.Name });
        var queryPermissions = queryTypes.Select(x => new Permission { Name = x.QueryType.Name });
        var allNewPermissions = new List<Permission>();
        allNewPermissions.AddRange(commandPermissions);
        allNewPermissions.AddRange(queryPermissions);

        var permissions = dbContext.Permissions.ToList();

        if (permissions is null || permissions.Count == 0)
        {
            dbContext.Permissions.AddRange(commandPermissions);
            dbContext.Permissions.AddRange(queryPermissions);
        }
        else
        {
            var diffPermissions = permissions.Except(allNewPermissions, new PermissionEqualityComparer());
            if (diffPermissions.Any())
            {
                dbContext.Permissions.AddRange(diffPermissions);
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken: CancellationToken.None);
    }
}

public class PermissionEqualityComparer : IEqualityComparer<Permission>
{
    public bool Equals(Permission? x, Permission? y)
    {
        return x.Name == y.Name;
    }

    public int GetHashCode([DisallowNull] Permission obj)
    {
        return obj.Name.GetHashCode();
    }
}

