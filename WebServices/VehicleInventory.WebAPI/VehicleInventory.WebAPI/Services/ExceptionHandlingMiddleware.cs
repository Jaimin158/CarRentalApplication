using System.Net;
using VehicleInventory.Domain.Exceptions;

namespace VehicleInventory.WebAPI.Services
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        
        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DomainException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;

                await context.Response.WriteAsJsonAsync(new
                {
                    error = ex.Message
                });
            }
        }
    }
}
