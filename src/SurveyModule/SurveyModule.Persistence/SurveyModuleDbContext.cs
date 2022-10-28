using Microsoft.EntityFrameworkCore;
using SurveyModule.Common;
using CoreModule.Domain;
using CoreModule.Domain.Permissions;
using CoreModule.Domain.Roles;
using CoreModule.Domain.Users;
using CoreModule.Application.Common.Interfaces;

namespace SurveyModule;

public class SurveyModuleDbContext : DbContext, ISurveyModuleDbContext
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;

    public SurveyModuleDbContext(DbContextOptions<SurveyModuleDbContext> options) : base(options)
    {
    }

    public SurveyModuleDbContext(
        DbContextOptions<SurveyModuleDbContext> options,
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
    public DbSet<Survey> Surveys { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreationUser = _currentUserService.UserId.HasValue ? _currentUserService.UserId.Value : default(int);
                    entry.Entity.CreationDate = _dateTime.Now;
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedUser = _currentUserService.UserId;
                    entry.Entity.ModifiedDate = _dateTime.Now;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SurveyModuleDbContext).Assembly);
    }
}

