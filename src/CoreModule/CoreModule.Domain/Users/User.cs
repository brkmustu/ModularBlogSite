namespace CoreModule.Domain.Users;

public class User : AuditableEntity
{
    public string UserName { get; private set; }
    public byte[] PasswordHash { get; private set; }
    public byte[] PasswordSalt { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string FullName => FirstName.Trim() + " " + LastName.Trim();
    public string MobileNumber { get; private set; }
    public string EmailAddress { get; private set; }
    public int UserStatusId { get; private set; }
    public long[] RoleIds { get; private set; }

    internal User() { }

    public User(string userName, string firstName, string lastName, long[] roleIds = default)
    {
        UserName = userName;
        FirstName = firstName;
        LastName = lastName;
        IsActive = false;
        UserStatusId = (int)UserStatusType.WaitingForApproval;
        RoleIds = roleIds;
    }

    public User SetPasswordHash(byte[] value)
    {
        PasswordHash = value;
        return this;
    }
    public User SetPasswordSalt(byte[] value)
    {
        PasswordSalt = value;
        return this;
    }
    public User SetUserStatus(UserStatusType userStatusType)
    {
        UserStatusId = (int)userStatusType;
        return this;
    }
    public void SetMobileNumber(string value)
    {
        MobileNumber = value;
    }
    public void SetEmailAddress(string value)
    {
        EmailAddress = value;
    }
}
