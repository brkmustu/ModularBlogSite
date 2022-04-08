using CoreModule.Domain.Users;

namespace CoreModule.Application.Common.MessageContracts;

public class UserRegisteredEvent
{
    public User User { get; set; }
    public string Password { get; set; }
}
