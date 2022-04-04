using CoreModule.Application.Common.Contracts;
using CoreModule.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json;

namespace CoreModule.Application.Auditing;

public class AuditingCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
{
    private readonly ILogger<AuditLogInfo> _logger;
    private readonly ICurrentUserService _currentUserService;
    private readonly ICommandHandler<TCommand> _decorate;

    public AuditingCommandHandlerDecorator(ILogger<AuditLogInfo> logger, ICurrentUserService currentUserService, ICommandHandler<TCommand> decorate)
    {
        _logger = logger;
        _currentUserService = currentUserService;
        _decorate = decorate;
    }

    public async Task<Result> Handle(TCommand command)
    {
        Result result;
        var auditLog = new AuditLogInfo();
        auditLog.ApplicationName = nameof(TCommand);
        auditLog.CorrelationId = Guid.NewGuid().ToString();
        auditLog.Request = JsonSerializer.Serialize(command);
        if (_currentUserService.IsAuthenticated)
            auditLog.UserId = _currentUserService.UserId;

        var stopwatch = Stopwatch.StartNew();

        try
        {
            result = await _decorate.Handle(command);
            if (result != null)
                auditLog.Response = JsonSerializer.Serialize(result);
        }
        catch (Exception ex)
        {
            auditLog.Exceptions.Add(ex);
            result = Result.Failure(auditLog.Exceptions.Select(x => x.ToString()));
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
