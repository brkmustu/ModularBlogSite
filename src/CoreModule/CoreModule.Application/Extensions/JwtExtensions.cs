using CoreModule.Application.Common.Contracts;
using CoreModule.Domain.Users;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CoreModule.Application.Extensions;

public static class JwtExtensions
{
    public static AccessToken CreateToken(
            this User user, 
            IEnumerable<string> permissions, 
            TokenOptions tokenOptions
        )
    {
        var accessTokenExpiration = DateTime.Now.AddMinutes(tokenOptions.AccessTokenExpiration);
        var securityKey = CreateSecurityKey(tokenOptions.SecurityKey);
        var signingCredentials = CreateSigningCredentials(securityKey);
        var jwt = CreateJwtSecurityToken(tokenOptions, user, signingCredentials, permissions, accessTokenExpiration);
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var token = jwtSecurityTokenHandler.WriteToken(jwt);

        return new AccessToken()
        {
            Token = token,
            Expiration = accessTokenExpiration
        };
    }

    internal static JwtSecurityToken CreateJwtSecurityToken(
            TokenOptions tokenOptions, 
            User user,
            SigningCredentials signingCredentials, 
            IEnumerable<string> permissions,
            DateTime accessTokenExpiration
        )
    {
        var jwt = new JwtSecurityToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: SetClaims(user, permissions),
                signingCredentials: signingCredentials
        );
        return jwt;
    }

    private static IEnumerable<Claim> SetClaims(User user, IEnumerable<string> permissions)
    {
        var claims = new List<Claim>();
        claims.AddNameIdentifier(user.Id.ToString());
        if (!string.IsNullOrEmpty(user.FullName))
            claims.AddName($"{user.FullName}");
        claims.AddRoles(permissions.ToArray());

        return claims;
    }

    internal static SecurityKey CreateSecurityKey(string securityKey)
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
    }

    internal static SigningCredentials CreateSigningCredentials(SecurityKey securityKey)
    {
        return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
    }
}
