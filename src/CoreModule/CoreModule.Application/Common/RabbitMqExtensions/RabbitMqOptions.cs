namespace CoreModule.Application.Common.RabbitMqExtensions;

public class RabbitMqOptions
{
    public const string SectionName = "RabbitMqOptions";
    public string HostName { get; set; }
    public string VirtualHost { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}
