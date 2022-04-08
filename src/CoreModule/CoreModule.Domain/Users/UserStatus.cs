namespace CoreModule.Domain.Users;

public class UserStatus : ActivetableEntity, IEntity<int>
{
    public int Id { get; set; }
    public string Value { get; private set; }
    public UserStatus(string value)
    {
        Value = value;
    }
}

public enum UserStatusType
{
    Active = 1,
    WaitingForApproval = 2,
    Rejected = 3,
    Inactive = 4,
    Deleted = 5,
}
