using CoreModule;
using CoreModule.Application.Common.RabbitMqExtensions;
using Microsoft.Extensions.Configuration;

public static class CommonSettings
{
    public static bool? GetSeedSampleDataValue()
    {
        var value = Environment.GetEnvironmentVariable("SystemOptions__SeedSampleData");
        return value.IsNullOrEmpty() ? (bool?)null : bool.Parse(value);
    }

    public static string GetApiGatewayUrl()
    {
        string url = "http://localhost:5000/";
        var environmentValue = Environment.GetEnvironmentVariable("CommonSettings__ApiGatewayUrl");
        return environmentValue.IsNullOrEmpty() ? url : environmentValue;
    }

    public static string GetTokenValidationApiUrl(string accessToken)
    {
        string url = "http://localhost:5020/api/userPortal/auth";
        var environmentValue = Environment.GetEnvironmentVariable("CommonSettings__TokenValidationApiUrl");
        return (environmentValue.IsNullOrEmpty() ? url : environmentValue) + "/validate?token=" + accessToken;
    }

    public static string GetConsulAddress(IConfiguration configuration)
    {
        var address = configuration["ConsulConfig:Address"];
        var environmentValue = Environment.GetEnvironmentVariable("ConsulConfig__Address");
        return (environmentValue.IsNullOrEmpty() ? address : environmentValue);
    }

    public static string GetEnvironmentServiceName()
    {
        return Environment.GetEnvironmentVariable("ServiceName");
    }
    public static string GetEnvironmentServicePort()
    {
        return Environment.GetEnvironmentVariable("ServicePort");
    }

    public static RabbitMqOptions GetRabbitMqOptions(IConfiguration configuration)
    {
        var appSettingsSection = configuration.GetSection(RabbitMqOptions.SectionName);
        var options = appSettingsSection.Get<RabbitMqOptions>();

        var hostName = Environment.GetEnvironmentVariable("RabbitMqOptions__HostName");
        var virtualHost = Environment.GetEnvironmentVariable("RabbitMqOptions__VirtualHost");
        var userName = Environment.GetEnvironmentVariable("RabbitMqOptions__UserName");
        var password = Environment.GetEnvironmentVariable("RabbitMqOptions__Password");

        if (!hostName.IsNullOrEmpty())
        {
            options.HostName = hostName;
        }
        if (!virtualHost.IsNullOrEmpty())
        {
            options.VirtualHost = virtualHost;
        }
        if (!userName.IsNullOrEmpty())
        {
            options.UserName = userName;
        }
        if (!password.IsNullOrEmpty())
        {
            options.Password = password;
        }

        return options;

    }
}
