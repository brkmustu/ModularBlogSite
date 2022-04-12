using CoreModule.Application.Common.MessageContracts;
using ManagementModule.Common;
using MassTransit;
using CoreModule.Domain.Permissions;

namespace ManagementModule.Consumers;

public class SyncManagementDbPermissionConsumer : IConsumer<SyncManagementPermissionsEvent>
{
    private readonly IManagementModuleDbContext _dbContext;

    public SyncManagementDbPermissionConsumer(IManagementModuleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<SyncManagementPermissionsEvent> context)
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
