namespace CoreModule.Application.Auditing;

public class AuditLogInfo
{
    public string ApplicationName { get; set; }
    public string CorrelationId { get; set; }
    public Guid? UserId { get; set; }
    public string UserName { get; set; }
    public DateTime ExecutionTime { get; set; }
    public int ExecutionDuration { get; set; }
    public string ClientIpAddress { get; set; }
    public string Request { get; set; }
    public string Response { get; set; }

    public List<Exception> Exceptions { get; }

    public AuditLogInfo()
    {
        Exceptions = new List<Exception>();
    }
}
