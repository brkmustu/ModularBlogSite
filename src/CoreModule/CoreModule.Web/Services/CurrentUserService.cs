using CoreModule.Application.Common.Interfaces;
using System.Security.Claims;
using System.Security.Principal;

namespace CoreModule.Web.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor httpContextAccessor;
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }
    private string userId => this.httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    public Guid? UserId => IsAuthenticated ? new Guid(userId) : (Guid?)null;
    public bool IsAuthenticated => !userId.IsNullOrEmpty();
    public IIdentity Identity => this.Principal.Identity!;
    public bool IsInRole(string role) => this.Principal.IsInRole(role);
    private IPrincipal Principal => this.httpContextAccessor.HttpContext?.User!;
}
