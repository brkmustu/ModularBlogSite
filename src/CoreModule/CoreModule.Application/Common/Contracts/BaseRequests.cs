namespace CoreModule.Application.Common.Contracts;

public abstract class BaseRequest
{

    /// <summary>
    /// Uygulanacak endişelerin dizisidir.
    /// Örn: Authorization uygulanmasını istediğimiz 
    /// </summary>
    public virtual CrossCuttingConcerns[] ApplicableConcerns { get; } = new CrossCuttingConcerns[] { CrossCuttingConcerns.Validation };
    public abstract string OperationName { get; }
    public Guid CorrelationId { get; } = Guid.NewGuid();
}
public abstract class CommandRequest : BaseRequest, ICommand
{
}
public abstract class QueryRequest<TResult> : BaseRequest, IQuery<TResult>
{
}
