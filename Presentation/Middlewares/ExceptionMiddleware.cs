using ChatApp.Domain.Exceptions;

namespace ChatApp.Presentation.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "Handled AppException: {ErrorCode}", ex.ErrorCode);

                context.Response.StatusCode = ex.StatusCode;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    code = ex.StatusCode,
                    message = ex.Message,
                    errors = ex.GetErrors() ?? new List<string>()
                };

                await context.Response.WriteAsJsonAsync(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    code = 500,
                    message = "Internal server error",
                    errors = new[] { ex.Message }
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }

}
