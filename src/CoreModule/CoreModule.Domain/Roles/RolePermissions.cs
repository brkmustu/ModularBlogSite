using System.ComponentModel.DataAnnotations;

namespace CoreModule.Domain.Roles;

public class RolePermissions : ValueObject, IEntity<long>
{
    public long Id { get; set; }
    public long RoleId { get; set; }
    public long PermissionId { get; set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return RoleId;
        yield return PermissionId;
    }
}
