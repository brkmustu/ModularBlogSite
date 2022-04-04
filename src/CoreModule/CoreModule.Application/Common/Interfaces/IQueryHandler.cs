using CoreModule.Application.Common.Contracts;

namespace CoreModule.Application.Common.Interfaces;

public interface IQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    Task<TResult> Handle(TQuery query);
}
