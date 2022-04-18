using CoreModule.Application.Common.Interfaces;

namespace CoreModule.Application.Common;

public class AppGuid : IAppGuid
{
    private Guid _appId = Guid.NewGuid();
    public Guid AppId => _appId;
}
