using CoreModule.Application.Common.Contracts;
using CoreModule.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json;

namespace CoreModule.Application.Auditing;

public class AuditingQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    private readonly ILogger<AuditLogInfo> _logger;
    private readonly ICurrentUserService _currentUserService;
    private readonly IQueryHandler<TQuery, TResult> _decorate;

    public AuditingQueryHandlerDecorator(ILogger<AuditLogInfo> logger, ICurrentUserService currentUserService, IQueryHandler<TQuery, TResult> decorate)
    {
        _logger = logger;
        _currentUserService = currentUserService;
        _decorate = decorate;
    }

    public async Task<TResult> Handle(TQuery query)
    {
        var result = default(TResult);
        var auditLog = new AuditLogInfo();
        auditLog.ApplicationName = nameof(TQuery);
        auditLog.CorrelationId = Guid.NewGuid().ToString();
        auditLog.Request = JsonSerializer.Serialize(query);
        if (_currentUserService.IsAuthenticated)
            auditLog.UserId = _currentUserService.UserId;

        var stopwatch = Stopwatch.StartNew();

        try
        {
            result = await _decorate.Handle(query);
            if (result != null)
                auditLog.Response = JsonSerializer.Serialize(result);
        }
        catch (Exception ex)
        {
            auditLog.Exceptions.Add(ex);
            throw;
        }
        finally
        {
            stopwatch.Stop();
            auditLog.ExecutionDuration = Convert.ToInt32(stopwatch.Elapsed.TotalMilliseconds);
            if (auditLog.Exceptions.Any())
                _logger.LogError(auditLog.Exceptions.FirstOrDefault(), JsonSerializer.Serialize(auditLog));
            else
                _logger.LogInformation(JsonSerializer.Serialize(auditLog));
        }
        return result;
    }
}
