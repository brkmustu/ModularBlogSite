using CoreModule.Application.Common.Interfaces;

namespace CoreModule.Web.Codes;

public sealed record Commands(IServiceProvider ServiceProvider)
{
    public async Task<IResult> InvokeAsync<TCommand>(TCommand command)
    {
        var handler = ServiceProvider.GetService<ICommandHandler<TCommand>>();

        if (handler is null)
        {
            return await Task.FromResult(Results.BadRequest());
        }

        try
        {
            var result = await handler.Handle(command);
            return await Task.FromResult(Results.Ok());
        }
        catch (Exception exception)
        {
            var response = WebApiErrorResponseBuilder.CreateErrorResponseOrNull(exception);

            if (response != null)
            {
                return await Task.FromResult(response);
            }
            else
            {
                throw;
            }
        }
    }
}
