using System.Security.Principal;

namespace CoreModule.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        bool IsAuthenticated { get; }
        IIdentity Identity { get; }
        bool IsInRole(string role);
    }
}
