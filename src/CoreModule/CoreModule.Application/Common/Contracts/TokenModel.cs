namespace CoreModule.Application.Common.Contracts;

public class TokenModel
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public List<OperationClaim> Claims { get; set; }
}
