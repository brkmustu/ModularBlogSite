namespace CoreModule.Domain.Permissions;

public class Permission : IEntity<long>
{
    public long Id { get; set; }
    public string Name { get; set; }
}
