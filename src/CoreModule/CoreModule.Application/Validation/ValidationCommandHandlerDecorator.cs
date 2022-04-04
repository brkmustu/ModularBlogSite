using CoreModule.Application.Common.Contracts;
using CoreModule.Application.Common.Interfaces;

namespace CoreModule.Application.Validation;

public class ValidationCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
{
    private readonly ICommandHandler<TCommand> _decorate;
    private readonly IValidator _validator;

    public ValidationCommandHandlerDecorator(ICommandHandler<TCommand> decorate, IValidator validator)
    {
        _decorate = decorate;
        _validator = validator;
    }

    public Task<Result> Handle(TCommand command)
    {
        if (command == null) throw new ArgumentNullException(nameof(command));

        // validate the supplied command.
        _validator.ValidateObject(command);

        return _decorate.Handle(command);
    }
}
