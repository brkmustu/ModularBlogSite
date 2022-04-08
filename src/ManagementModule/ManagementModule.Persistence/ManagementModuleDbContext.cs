using Microsoft.EntityFrameworkCore;
using ManagementModule.Common;
using CoreModule.Domain;
using CoreModule.Domain.Permissions;
using CoreModule.Domain.Roles;
using CoreModule.Domain.Users;
using CoreModule.Application.Common.Interfaces;
using CoreModule.Application;
using CoreModule.Application.Common;

namespace ManagementModule;

public class ManagementModuleDbContext : DbContext, IManagementModuleDbContext
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;

    public ManagementModuleDbContext(DbContextOptions<ManagementModuleDbContext> options)
        : base(options)
    {
        _currentUserService = new NullCurrentUserService();
        _dateTime = new MachineDateTime();
    }

    public ManagementModuleDbContext(
        DbContextOptions<ManagementModuleDbContext> options,
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
                    if (_currentUserService is not null)
                        entry.Entity.CreatedBy = _currentUserService.UserId.HasValue ? _currentUserService.UserId.Value : Guid.Empty;
                    else
                        entry.Entity.CreatedBy = SystemOptions.SystemGuid;
                    entry.Entity.CreatedDate = _dateTime.Now;
                    break;
                case EntityState.Modified:
                    if (_currentUserService is not null)
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                    else
                        entry.Entity.LastModifiedBy = SystemOptions.SystemGuid;
                    entry.Entity.LastModifiedDate = _dateTime.Now;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ManagementModuleDbContext).Assembly);
    }
}

