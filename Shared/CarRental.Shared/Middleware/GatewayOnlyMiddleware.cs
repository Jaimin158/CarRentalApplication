using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CarRental.Shared.Middleware
{
    public class GatewayOnlyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string HeaderName = "X-Gateway-Internal";

        public GatewayOnlyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
        {
            var expectedValue = configuration["GatewayInternalKey"];

            if (!context.Request.Headers.TryGetValue(HeaderName, out var providedValue) ||
                string.IsNullOrWhiteSpace(expectedValue) ||
                providedValue != expectedValue)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(new
                {
                    error = "Forbidden",
                    message = "Direct access to this API is not allowed."
                });

                return;
            }

            await _next(context);
        }
    }
}
