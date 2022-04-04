using System.ComponentModel.DataAnnotations;

namespace CoreModule.Domain.Roles;

public class Role : ActivetableEntity, IEntity<long>
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public long[] PermissionIds { get; set; }
}
