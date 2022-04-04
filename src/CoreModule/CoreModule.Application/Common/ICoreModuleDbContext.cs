using CoreModule.Domain.Permissions;
using CoreModule.Domain.Roles;
using CoreModule.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace CoreModule.Application.Common
{
    public interface ICoreModuleDbContext
    {
        DbSet<Permission> Permissions { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<User> Users { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
