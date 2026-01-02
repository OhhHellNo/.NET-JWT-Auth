using WebApplication1.Exceptions;
using WebApplication1.Models.Api;

namespace WebApplication1.Middlewares
{
    public class ExceptionMiddleware
    {
        
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ApiException ex)
            {
                context.Response.StatusCode = ex.StatusCode;
                context.Response.ContentType = "application/json";

                var response = new ApiError
                {
                    StatusCode = ex.StatusCode,
                    Message = ex.Message,
                    Errors = ex.Errors
                };

                await context.Response.WriteAsJsonAsync(response);
            }
            catch (Exception)
            {
                context.Response.StatusCode = 500;

                var response = new ApiError
                {
                    StatusCode = 500,
                    Message = "Internal Server Error"
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }

}

