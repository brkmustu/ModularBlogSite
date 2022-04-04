using CoreModule.Application.Common.Contracts;

namespace CoreModule.Application.Common.Interfaces;

public interface ICommandHandler<TCommand>
{
    Task<Result> Handle(TCommand command);
}
