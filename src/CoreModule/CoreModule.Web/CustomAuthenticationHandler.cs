using CoreModule.Application;
using CoreModule.Application.Common.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace CoreModule.Web;

public class CustomAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly HttpClient _client;
    private readonly HttpContext _httpContext;

    public CustomAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IHttpClientFactory httpClientFactory,
        IHttpContextAccessor httpContextAccessor
        ) : base(options, logger, encoder, clock)
    {
        _client = httpClientFactory.CreateClient();
        _httpContext = httpContextAccessor.HttpContext;
    }

    protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (_httpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            var accessToken = authHeader.ToString().Split(' ')[1];

            var response = await _client.GetAsync(CommonSettings.GetTokenValidationApiUrl(accessToken));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                var tokenModel = JsonConvert.DeserializeObject<TokenModel>(result);
                
                return AuthenticateResult.Success(
                    new AuthenticationTicket(
                        new ClaimsPrincipal(
                            new ClaimsIdentity(
                                tokenModel.Claims.Select(x => new Claim(x.ClaimType, x.Value)))),
                    AppConsts.DefaultAuthenticationSchemeName));
            }
        }
        return AuthenticateResult.Fail("Failed Authentication");
    }
}
