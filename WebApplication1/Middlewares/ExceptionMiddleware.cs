namespace WebApplication1.Middlewares
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

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro inesperado: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            // Se a resposta já começou a ser enviada, não podemos redirecionar
            if (context.Response.HasStarted)
            {
                return Task.CompletedTask;
            }

            context.Response.StatusCode = 500; // Internal Server Error
            //context.Response.Redirect($"/Home/Error?message={(ex.Message)}");
            context.Response.Redirect("/Home/Error");

            return Task.CompletedTask;
        }
    }
}
