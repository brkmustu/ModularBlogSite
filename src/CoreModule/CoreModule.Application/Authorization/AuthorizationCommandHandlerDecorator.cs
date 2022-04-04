using CoreModule.Application.Common.Contracts;
using CoreModule.Application.Common.Interfaces;
using System.Security;

namespace CoreModule.Application.Authorization;

public class AuthorizationCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
{
    private readonly ICommandHandler<TCommand> decoratedHandler;
    private readonly ICurrentUserService currentUser;

    public AuthorizationCommandHandlerDecorator(ICommandHandler<TCommand> decoratedHandler, ICurrentUserService currentUser)
    {
        this.decoratedHandler = decoratedHandler;
        this.currentUser = currentUser;
    }

    public Task<Result> Handle(TCommand query)
    {
        this.Authorize();

        return this.decoratedHandler.Handle(query);
    }

    private void Authorize()
    {
        if (!nameof(TCommand).EndsWith(AuthorizationConsts.AnonymousCommandEndsWith) && !this.currentUser.IsInRole(nameof(TCommand)))
        {
            throw new SecurityException();
        }
    }
}
