﻿using UserPortalModule.Common;
using CoreModule.Domain.Permissions;
using System.Diagnostics.CodeAnalysis;
using UserPortalModule.System.Permissions;

namespace UserPortalModule.System.SeedSampleData;

public class SeedPermissions
{
    private readonly IUserPortalModuleDbContext _dbContext;

    public SeedPermissions(IUserPortalModuleDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<List<long>> SyncAllAsync()
    {
        var allNewPermissions = PermissionExtensions.GetSystemPermissions();

        var permissions = _dbContext.Permissions.ToList();

        if (permissions is null || permissions.Count == 0)
        {
            _dbContext.Permissions.AddRange(allNewPermissions);
        }
        else
        {
            var diffPermissions = permissions.Except(allNewPermissions, new PermissionEqualityComparer());
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
