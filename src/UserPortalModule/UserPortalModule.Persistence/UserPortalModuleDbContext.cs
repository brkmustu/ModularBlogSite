using Microsoft.EntityFrameworkCore;
using UserPortalModule.Common;
using CoreModule.Domain;
using CoreModule.Domain.Permissions;
using CoreModule.Domain.Roles;
using CoreModule.Domain.Users;
using CoreModule.Application.Common.Interfaces;
using CoreModule.Application;
using CoreModule.Application.Common;
using CoreModule.Configurations;

namespace UserPortalModule;

public class UserPortalModuleDbContext : DbContext, IUserPortalModuleDbContext
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;

    public UserPortalModuleDbContext(DbContextOptions<UserPortalModuleDbContext> options)
        : base(options)
    {
        _currentUserService = new NullCurrentUserService();
        _dateTime = new MachineDateTime();
    }

    public UserPortalModuleDbContext(
        DbContextOptions<UserPortalModuleDbContext> options,
        ICurrentUserService currentUserService,
        IDateTime dateTime)
        : base(options)
    {
        _currentUserService = currentUserService;
        _dateTime = dateTime;
    }

    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    if (entry.Entity.CreationUser == Guid.Empty)
                    {
                        if (_currentUserService is not null)
                            entry.Entity.CreationUser = _currentUserService.UserId.HasValue ? _currentUserService.UserId.Value : SystemOptions.SystemGuid;
                        else
                            entry.Entity.CreationUser = SystemOptions.SystemGuid;
                    }
                    entry.Entity.CreationDate = _dateTime.Now;
                    break;
                case EntityState.Modified:
                    if (!entry.Entity.ModifiedUser.HasValue || entry.Entity.ModifiedUser.Value == Guid.Empty)
                    {
                        if (_currentUserService is not null && _currentUserService.UserId.HasValue)
                            entry.Entity.ModifiedUser = _currentUserService.UserId;
                        else
                            entry.Entity.ModifiedUser = SystemOptions.SystemGuid;
                    }
                    entry.Entity.ModifiedDate = _dateTime.Now;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserPortalModuleDbContext).Assembly);
    }
}

