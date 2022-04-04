namespace CoreModule.Domain;

public interface IEntity : IEntity<Guid>
{
}
public interface IEntity<TPrimaryKey>
{
    TPrimaryKey Id { get; set; }
}
public class Entity<TPrimaryKey> : ActivetableEntity, IEntity<TPrimaryKey>
{
    public TPrimaryKey Id { get; set; }

}
public class Entity : Entity<Guid>
{
}

public abstract class ActivetableEntity
{
    public bool IsActive { get; protected set; }

    public void Activate()
    {
        this.IsActive = true;
    }
    public void Deactivate()
    {
        this.IsActive = false;
    }
}
