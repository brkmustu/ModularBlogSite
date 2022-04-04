using CoreModule.Application.Common.Contracts;
using CoreModule.Application.Common.Interfaces;

namespace CoreModule.Application.Validation;

public class ValidationQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    private readonly IQueryHandler<TQuery, TResult> _decorate;
    private readonly IValidator _validator;

    public ValidationQueryHandlerDecorator(IQueryHandler<TQuery, TResult> decorate, IValidator validator)
    {
        _decorate = decorate;
        _validator = validator;
    }

    public Task<TResult> Handle(TQuery query)
    {
        if (query == null) throw new ArgumentNullException(nameof(query));

        // validate the supplied command.
        _validator.ValidateObject(query);

        return _decorate.Handle(query);
    }
}
