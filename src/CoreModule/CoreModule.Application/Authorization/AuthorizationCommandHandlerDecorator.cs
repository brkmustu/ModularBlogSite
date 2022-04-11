using CoreModule.Application.Common.Contracts;
using CoreModule.Application.Common.Interfaces;
using System.Security;

namespace CoreModule.Application.Authorization;

public class AuthorizationCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
    where TCommand : CommandRequest
{
    private readonly ICommandHandler<TCommand> decoratedHandler;
    private readonly ICurrentUserService currentUser;

    public AuthorizationCommandHandlerDecorator(ICommandHandler<TCommand> decoratedHandler, ICurrentUserService currentUser)
    {
        this.decoratedHandler = decoratedHandler;
        this.currentUser = currentUser;
    }

    public Task<Result> Handle(TCommand command)
    {
        this.Authorize(command);

        return this.decoratedHandler.Handle(command);
    }

    private void Authorize(TCommand command)
    {
        if (command.ApplicableConcerns.Contains(CrossCuttingConcerns.Authorization) 
            && !this.currentUser.IsInRole(command.OperationName))
        {
            throw new SecurityException();
        }
    }
}
