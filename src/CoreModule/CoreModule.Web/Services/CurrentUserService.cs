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
        var userId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        IsAuthenticated = userId != null && !userId.IsNullOrEmpty();
        if (IsAuthenticated)
            UserId = new Guid(userId);
    }
    public Guid? UserId { get; }
    public bool IsAuthenticated { get; }
    public IIdentity Identity => this.Principal.Identity!;
    public bool IsInRole(string role) => this.Principal.IsInRole(role);
    private IPrincipal Principal => this.httpContextAccessor.HttpContext?.User!;
}
