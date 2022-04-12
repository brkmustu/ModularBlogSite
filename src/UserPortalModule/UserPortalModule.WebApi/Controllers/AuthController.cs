using CoreModule.Application.Common.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Text;

namespace UserPortalModule.WebApi.Controllers;

[ApiController]
public class AuthController : Controller
{
    private readonly TokenOptions _tokenOptions;

    public AuthController(IOptions<TokenOptions> options)
    {
        _tokenOptions = options.Value;
    }

    [HttpGet]
    [Route("api/[controller]/[action]")]
    public async Task<TokenModel> Validate(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = _tokenOptions.Issuer,
                ValidAudience = _tokenOptions.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.SecurityKey)),
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userName = jwtToken.Claims.First(x => x.Type == ClaimTypes.Name).Value;
            var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

            // return account id from JWT token if validation successful
            return new TokenModel { UserName = userName, UserId = userId, Claims = jwtToken.Claims.Select(x => new OperationClaim { Issuer = x.Issuer, ClaimType = x.Type, Value = x.Value } ).ToList()};
        }
        catch
        {
            // return null if validation fails
            throw new SecurityException("Kimlik doğrulaması başarısız!");
        }
    }
}
