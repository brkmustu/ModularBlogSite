using CoreModule.Application.Common.Contracts;

namespace UserPortalModule.Common.Contracts;

public abstract class QueryRequest<TResult> : BaseQueryRequest<TResult>
{
    public override string ModuleName => ModuleConsts.ModuleName;
}
