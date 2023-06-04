using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alea.Auth
{
    public class ApiKeyAuthFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string apiKey = _configuration.GetValue<string>(AuthConstants.ApiKeySectionName);

            if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var extractedApiKey))
            {
                context.Result = new UnauthorizedObjectResult("Nema API ključa");
                return;
            }
            //else await context.Response.WriteAsync("Ima API ključa");

            if (!apiKey.Equals(extractedApiKey))
            {
                context.Result = new UnauthorizedObjectResult("Loš API ključa");
                return;
            }
            //await context.Response.WriteAsync("Dobar API ključ");

            //await _next(context);
        }

        private readonly IConfiguration _configuration;

        public ApiKeyAuthFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
