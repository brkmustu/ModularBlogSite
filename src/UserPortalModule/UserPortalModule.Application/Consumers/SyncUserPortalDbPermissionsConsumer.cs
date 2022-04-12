using CoreModule.Application.Common.MessageContracts;
using UserPortalModule.Common;
using MassTransit;
using CoreModule.Domain.Permissions;

namespace UserPortalModule.Consumers;

public class SyncUserPortalDbPermissionsConsumer : IConsumer<SyncUserPortalPermissionsEvent>
{
    private readonly IUserPortalModuleDbContext _dbContext;

    public SyncUserPortalDbPermissionsConsumer(IUserPortalModuleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<SyncUserPortalPermissionsEvent> context)
    {
        var allNewPermissions = context.Message.Permissions.Select(permissionName => new Permission { Name = permissionName });
        var permissions = _dbContext.Permissions.ToList();

        if (permissions is null || permissions.Count == 0)
        {
            _dbContext.Permissions.AddRange(allNewPermissions);
            await _dbContext.SaveChangesAsync(cancellationToken: CancellationToken.None);
            var allPermissions = _dbContext.Permissions.ToList();
            var adminRole = _dbContext.Roles.FirstOrDefault(x => x.Name == "Admin");
            adminRole.PermissionIds = allPermissions.Select(x => x.Id).ToArray();
            _dbContext.Roles.Update(adminRole);
            await _dbContext.SaveChangesAsync(cancellationToken: CancellationToken.None);
        }
        else
        {
            var diffPermissions = allNewPermissions.Except(permissions, new PermissionNameComparer());
            if (diffPermissions.Any())
            {
                _dbContext.Permissions.AddRange(diffPermissions);
                await _dbContext.SaveChangesAsync(cancellationToken: CancellationToken.None);
                var allPermissions = _dbContext.Permissions.ToList();
                var adminRole = _dbContext.Roles.FirstOrDefault(x => x.Name == "Admin");
                adminRole.PermissionIds = allPermissions.Select(x => x.Id).ToArray();
                _dbContext.Roles.Update(adminRole);
                await _dbContext.SaveChangesAsync(cancellationToken: CancellationToken.None);
            }
        }
    }
}
