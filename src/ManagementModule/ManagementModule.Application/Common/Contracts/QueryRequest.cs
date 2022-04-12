using CoreModule.Application.Common.Contracts;

namespace ManagementModule.Common.Contracts;

public abstract class QueryRequest<TResult> : BaseQueryRequest<TResult>
{
    public override string ModuleName => ModuleConsts.ModuleName;
}
