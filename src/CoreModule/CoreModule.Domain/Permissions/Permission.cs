using System.Diagnostics.CodeAnalysis;

namespace CoreModule.Domain.Permissions;

public class Permission : IEntity<long>
{
    public long Id { get; set; }
    public string Name { get; set; }
}

public class PermissionNameComparer : IEqualityComparer<Permission>
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
