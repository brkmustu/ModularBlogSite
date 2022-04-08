using CoreModule.Application.Common.MessageContracts;
using CoreModule.Application.Extensions.Hashing;
using CoreModule.Domain.Permissions;
using ManagementModule.Common;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace ManagementModule.Consumers;

public class UserRegisteredEventConsumer : IConsumer<UserRegisteredEvent>
{
    private readonly ILogger<UserRegisteredEventConsumer> _logger;
    private readonly IManagementModuleDbContext _dbContext;
    public UserRegisteredEventConsumer(IManagementModuleDbContext dbContext, ILogger<UserRegisteredEventConsumer> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
    {
        var portalUser = context.Message.User;

        var encryptedPassword = context.Message.Password.CreatePasswordHash();

        /// portal kullanıcısının management modülünde herhangi bir yetkisi varsayılan olarak olmayacağı için rolleri siliyoruz.

        portalUser.SetPasswordHash(encryptedPassword.PasswordHash);
        portalUser.SetPasswordSalt(encryptedPassword.PasswordSalt);

        var portalRole = _dbContext.Roles.Where(x => x.Name == PermissionNames.Portal).FirstOrDefault();
        if (portalRole is not null)
            portalUser.SetRoles(new long[] { portalRole.Id });
        else
            portalUser.SetRoles(new long[] { });

        _dbContext.Users.Add(portalUser);

        await _dbContext.SaveChangesAsync(context.CancellationToken);

        _logger.LogInformation($"Kullanıcı yönetici onayına sunulmuştur. UserId: {portalUser.Id} | UserName: {portalUser.UserName}");
    }
}