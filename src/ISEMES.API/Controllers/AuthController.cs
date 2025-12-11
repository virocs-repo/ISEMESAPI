using ISEMES.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using ISEMES.Models;
using ISEMES.API.Services;

namespace ISEMES.API.Controllers
{
    [Route("api/v1/ise/auth/")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "inventory")]
    public class AuthController : ControllerBase
    {
        private readonly TokenProvider _tokenProvider;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthController(TokenProvider tokenProvider, IUserService userService, IConfiguration configuration)
        {
            _tokenProvider = tokenProvider;
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            try
            {
                if (login.External)
                {
                    var roles = await _userService.GetRoles(null, login.Username, login.Password);
                    var userType = "Ext";
                    var jwtToken = _tokenProvider.GenerateToken(login.Username, userType);
                    return Ok(new { Token = jwtToken, login.Username, UserType = userType, Roles = roles });
                }
                else
                {
                    if (ValidateAzureAdToken(login.IdToken))
                    {
                        var roles = await _userService.GetRoles(login.Email, null, null);
                        var userType = "Int";
                        var jwtToken = _tokenProvider.GenerateTokenFromAzureAdToken(login.IdToken, userType);
                        return Ok(new { Token = jwtToken, Username = roles.MainMenuItem[0].UserName, UserType = userType, Roles = roles });
                    }
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }
        }

        private bool ValidateAzureAdToken(string idToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tenantId = _configuration["AzureAd:TenantId"];
            var clientId = _configuration["AzureAd:ClientId"];

            var authority = $"https://login.microsoftonline.com/{tenantId}/v2.0";
            var wellKnownConfigUrl = $"{authority}/.well-known/openid-configuration";

            var configurationManager = new Microsoft.IdentityModel.Protocols.ConfigurationManager<OpenIdConnectConfiguration>(
                wellKnownConfigUrl, new OpenIdConnectConfigurationRetriever());

            OpenIdConnectConfiguration openIdConfig;
            try
            {
                openIdConfig = configurationManager.GetConfigurationAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching OpenID configuration: {ex.Message}");
                return false;
            }

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuers = new[] { $"{authority}" },
                ValidateAudience = true,
                ValidAudience = clientId,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(5),
                IssuerSigningKeys = openIdConfig.SigningKeys,
                ValidateIssuerSigningKey = true
            };

            try
            {
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(idToken, validationParameters, out validatedToken);
                return principal != null;
            }
            catch (SecurityTokenExpiredException)
            {
                return false;
            }
            catch (SecurityTokenValidationException ex)
            {
                Console.WriteLine($"Token validation failed: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool External { get; set; }
        public string IdToken { get; set; }
    }
}

