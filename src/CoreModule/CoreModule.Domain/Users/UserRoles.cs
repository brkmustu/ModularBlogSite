using System.ComponentModel.DataAnnotations;

namespace CoreModule.Domain.Users;

public class UserRoles : ValueObject, IEntity<long>
{
    [Key]
    public long Id { get; set; }
    public Guid UserId { get; set; }
    public long RoleId { get; set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return UserId;
        yield return RoleId;
    }
}
