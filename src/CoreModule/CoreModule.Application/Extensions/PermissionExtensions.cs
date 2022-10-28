using CoreModule.Application.Common.Interfaces;
using CoreModule.Application.CrossCuttingConcerns;
using CoreModule.Domain.Permissions;
using System.Reflection;

namespace CoreModule.Application.Extensions;

public static class PermissionExtensions
{
    public static List<Permission> GetAuthSystemPermissions(Assembly[] assemblies, string moduleName)
    {
        List<Type> handlerTypes = assemblies.SelectMany(x => x.GetTypes())
            .Where(x => x.GetInterfaces().Any(y => IsHandlerInterface(y)))
            .Where(x => x.Name.EndsWith("Handler"))
            .Where(x => x.GetCustomAttributes(false).Any(y => y.GetType() == typeof(AuthorizationDecoratorAttribute)))
            .ToList();

        var allNewPermissions = new List<Permission>();
        allNewPermissions.AddRange(handlerTypes.Select(x => new Permission { Name = moduleName + "." + x.ReflectedType.Name }));

        return allNewPermissions;
    }

    public static List<Permission> GetUserPermissions(ICoreModuleDbContext dbContext, int userId)
    {
        var user = dbContext.Users.FirstOrDefault(x => x.Id == userId);

        if (user == null)
        {
            return null;
        }

        if (user.RoleIds == null || user.RoleIds.Length == 0)
        {
            return null;
        }

        List<long> permissionIds = new List<long>();

        foreach (var roleId in user.RoleIds)
        {
            var role = dbContext.Roles.FirstOrDefault(x => x.Id == roleId);
            if (role == null)
                continue;
            permissionIds.AddRange(role.PermissionIds);
        }

        List<Permission> permissions = new List<Permission>();


        foreach (var item in permissionIds)
        {
            var permission = dbContext.Permissions.FirstOrDefault(x => x.Id == item);
            if (permission == null)
                continue;
            permissions.Add(permission);
        }

        return permissions;
    }

    private static bool IsHandlerInterface(Type type)
    {
        if (!type.IsGenericType)
            return false;

        Type typeDefinition = type.GetGenericTypeDefinition();

        return typeDefinition == typeof(ICommandHandler<>) || typeDefinition == typeof(IQueryHandler<,>);
    }
}
