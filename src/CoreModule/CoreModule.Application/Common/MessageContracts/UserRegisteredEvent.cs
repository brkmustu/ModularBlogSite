using CoreModule.Application.Common.Contracts;

namespace CoreModule.Application.Common.MessageContracts;

public class UserRegisteredEvent
{
    public UserDto User { get; set; }
    public string Password { get; set; }
}
