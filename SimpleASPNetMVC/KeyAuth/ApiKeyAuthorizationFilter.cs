using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SimpleASPNetMVC.KeyAuth
{
    public class ApiKeyAuthorizationFilter : IAuthorizationFilter
    {
        private readonly IConfiguration _configuration;

        public ApiKeyAuthorizationFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Check if the API key is provided in the request headers
            if (!context.HttpContext.Request.Headers.TryGetValue(Constants.ApiKeyHeader, out var apiKeyHeaderValues))
            {
                // API key is missing, return 401 Unauthorized
                context.Result = new UnauthorizedResult();
                return;
            }

            // Validate the API key
            var apiKey = apiKeyHeaderValues.ToString();
            if (!IsValidApiKey(apiKey))
            {
                // Invalid API key, return 401 Unauthorized
                context.Result = new UnauthorizedResult();
                return;
            }
        }

        private bool IsValidApiKey(string apiKey)
        {
            // Retrieve the API key from appSettings.json
            var expectedApiKey = _configuration[Constants.ApiKeyConfigName];

            // Validate the API key
            bool result = apiKey == expectedApiKey;
            return result;
        }

    }
}
