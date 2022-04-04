namespace CoreModule.Application.Common.Contracts;

public class UserDto
{
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MobileNumber { get; set; }
    public string EmailAddress { get; set; }
    public long[] RoleIds { get; set; }
}
