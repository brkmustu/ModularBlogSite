using System.Security.Principal;

namespace CoreModule.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        int? UserId { get; }
        bool IsAuthenticated { get; }
        IIdentity Identity { get; }
        bool IsInRole(string role);
    }
}
