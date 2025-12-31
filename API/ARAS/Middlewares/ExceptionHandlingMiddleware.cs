using System.Net;
using System.Text.Json;

namespace ARAS.Middlewares
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
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";

                var statusCode = (int)HttpStatusCode.InternalServerError;
                var message = "An unexpected error occurred. Please try again later.";

                // Handle specific exceptions
                if (ex is UnauthorizedAccessException)
                {
                    statusCode = (int)HttpStatusCode.Forbidden;
                    message = "You are not authorized to perform this action.";
                }
                else if (ex is KeyNotFoundException)
                {
                    statusCode = (int)HttpStatusCode.NotFound;
                    message = "The requested resource was not found.";
                }

                var response = new
                {
                    StatusCode = statusCode,
                    Message = message
                };

                context.Response.StatusCode = statusCode;
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
     }
}
