namespace CoreModule.Application.Common.Contracts;

public abstract class BaseRequest
{
    public virtual string OperationName { get; }
    public abstract string ModuleName { get; }
    public Guid CorrelationId { get; } = Guid.NewGuid();
}

public abstract class BaseCommandRequest : BaseRequest, ICommand { }
public abstract class BaseQueryRequest<TResult> : BaseRequest, IQuery<TResult> { }

public static class BaseRequestExtensions
{
    public static string GetPermissionName(this BaseRequest baseRequest)
    {
        return baseRequest.ModuleName + "." + baseRequest.OperationName;
    }
}
