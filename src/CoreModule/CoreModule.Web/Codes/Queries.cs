using CoreModule.Application.Common.Contracts;
using CoreModule.Application.Common.Interfaces;

namespace CoreModule.Web.Codes;

public sealed record Queries(IServiceProvider ServiceProvider)
{
    public async Task<TResult> InvokeAsync<TQuery, TResult>(HttpContext context, TQuery query)
        where TQuery : IQuery<TResult>
    {
        var handler = ServiceProvider.GetService<IQueryHandler<TQuery, TResult>>();

        if (handler is null)
        {
            return default;
        }

        try
        {
            TResult result = await handler.Handle(query);
            return result;
        }
        catch (Exception exception)
        {
            var response = WebApiErrorResponseBuilder.CreateErrorResponseOrNull(exception);

            if (response != null)
            {
                await response.ExecuteAsync(context);

                return default!;
            }
            else
            {
                throw;
            }
        }
    }
}
