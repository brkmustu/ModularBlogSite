namespace CoreModule.Application.Common.Contracts;

public class AccessToken
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
}
