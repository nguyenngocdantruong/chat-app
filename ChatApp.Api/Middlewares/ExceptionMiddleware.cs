using ChatApp.Domain.Exceptions;

namespace ChatApp.Api.Middlewares
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (AppException appException)
            {
                context.Response.StatusCode = appException.StatusCode;
                context.Response.ContentType = "application/json";

                var response = new {
                    CreatedAt  = DateTime.UtcNow,
                    Code = appException.StatusCode,
                    Message = appException.Message,
                    Success = false,
                    Errors = appException.Errors
                };

                await context.Response.WriteAsJsonAsync(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception occurred");

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    CreatedAt = DateTime.UtcNow,
                    Code = 500,
                    Message = ex.Message,
                    Success = false,
                    Errors = new {}
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }

}
