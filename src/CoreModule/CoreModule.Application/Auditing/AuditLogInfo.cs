using System.Text.Json.Serialization;

namespace CoreModule.Application.Auditing;

[JsonSerializable(typeof(AuditLogInfo))]
public class AuditLogInfo
{
    public string ApplicationName { get; set; }
    public string CorrelationId { get; set; }
    public int? UserId { get; set; }
    public string UserName { get; set; }
    public DateTime ExecutionTime { get; set; }
    public int ExecutionDuration { get; set; }
    public string ClientIpAddress { get; set; }
    public string Request { get; set; }
    public string Response { get; set; }
    [JsonIgnore]
    public List<Exception> Exceptions { get; }
    public IEnumerable<KeyValuePair<string, string>> ExceptionTexts => 
        Exceptions.Select(
            exception 
                => new KeyValuePair<string, string>(exception.ToString(), exception.InnerException is null ? "" : exception.InnerException.ToString())
            );

    public AuditLogInfo()
    {
        Exceptions = new List<Exception>();
    }
}
