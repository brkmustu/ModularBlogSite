using CoreModule.Application.Common.Contracts;

namespace ManagementModule.Common.Contracts;

public abstract class CommandRequest : BaseCommandRequest
{
    public override string ModuleName => ModuleConsts.ModuleName;
}
