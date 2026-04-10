namespace CarRentalApiGateway.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string HeaderName = "X-Api-Key";

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
        {
            var configuredApiKey = configuration["ApiKey"];

            if (!context.Request.Headers.TryGetValue(HeaderName, out var providedApiKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new
                {
                    error = "Unauthorized",
                    message = "API Key is missing."
                });
                return;
            }

            if (string.IsNullOrWhiteSpace(configuredApiKey) || providedApiKey != configuredApiKey)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new
                {
                    error = "Unauthorized",
                    message = "Invalid API Key."
                });
                return;
            }

            await _next(context);
        }
    }
}
