using CoreModule.Application.Common.Interfaces;
using System.Security.Principal;

namespace CoreModule.Application.Common;

public class NullCurrentUserService : ICurrentUserService
{
    public int? UserId => default(int);

    public bool IsAuthenticated => false;

    public IIdentity Identity => default;

    public bool IsInRole(string role)
    {
        return false;
    }
}
