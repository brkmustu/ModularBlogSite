using System.ComponentModel.DataAnnotations;

namespace CoreModule.Domain.Permissions;

public class Permission : IEntity<long>
{
    [Key]
    public long Id { get; set; }
    public string Name { get; set; }
}
