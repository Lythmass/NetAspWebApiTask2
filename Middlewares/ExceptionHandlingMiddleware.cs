using System.Net;

namespace Reddit.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _nextDelegate;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate nextDelegate, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _nextDelegate = nextDelegate;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _nextDelegate(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                if (ex is BadHttpRequestException)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
                await context.Response.WriteAsync(ex.Message);
            }
        }

    }
}
