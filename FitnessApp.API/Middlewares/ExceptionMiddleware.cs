using FluentValidation;
using System.Net;
using System.Text.Json;

namespace FitnessApp.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var message = "Sunucu kaynaklı bir hata oluştu.";


            if (exception is ValidationException validationException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                message = validationException.Message;
            }

            var result = JsonSerializer.Serialize(new
            {
                statusCode = context.Response.StatusCode,
                message = message
            });

            return context.Response.WriteAsync(result);
        }
    }
}