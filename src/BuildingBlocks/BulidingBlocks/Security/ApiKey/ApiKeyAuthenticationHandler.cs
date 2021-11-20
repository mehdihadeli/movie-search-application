using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace BuildingBlocks.Security.ApiKey
{
    //https://josef.codes/asp-net-core-protect-your-api-with-api-keys/
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        private const string ProblemDetailsContentType = "application/problem+json";
        private readonly IGetApiKeyQuery _getApiKeyQuery;

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<ApiKeyAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IGetApiKeyQuery getApiKeyQuery) : base(options, logger, encoder, clock)
        {
            _getApiKeyQuery = getApiKeyQuery ?? throw new ArgumentNullException(nameof(getApiKeyQuery));
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            StringValues apiKeyQueryValues = "";
            StringValues apiKeyHeaderValues = "";

            if (Request.Headers.TryGetValue(ApiKeyConstants.HeaderName, out apiKeyHeaderValues) == false &&
                Request.Query.TryGetValue(ApiKeyConstants.HeaderName, out apiKeyQueryValues) == false)
            {
                return AuthenticateResult.NoResult();
            }

            var providedApiKey = apiKeyHeaderValues.FirstOrDefault() ?? apiKeyQueryValues.FirstOrDefault();

            if ((apiKeyHeaderValues.Count == 0 && apiKeyQueryValues.Count == 0) || string.IsNullOrWhiteSpace
                (providedApiKey))
            {
                return AuthenticateResult.NoResult();
            }

            var existingApiKey = await _getApiKeyQuery.Execute(providedApiKey);

            if (existingApiKey != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, existingApiKey.Owner)
                };

                claims.AddRange(existingApiKey.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

                var identity = new ClaimsIdentity(claims, Options.AuthenticationType);
                var identities = new List<ClaimsIdentity> { identity };
                var principal = new ClaimsPrincipal(identities);
                var ticket = new AuthenticationTicket(principal, Options.Scheme);

                return AuthenticateResult.Success(ticket);
            }

            return AuthenticateResult.NoResult();
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 401;
            Response.ContentType = ProblemDetailsContentType;
            var problemDetails = new UnauthorizedProblemDetails();

            await Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
        }

        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 403;
            Response.ContentType = ProblemDetailsContentType;
            var problemDetails = new ForbiddenProblemDetails();

            await Response.WriteAsync(JsonSerializer.Serialize(problemDetails, DefaultJsonSerializerOptions.Options));
        }

        public static class DefaultJsonSerializerOptions
        {
            public static JsonSerializerOptions Options => new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = true
            };
        }
    }
}