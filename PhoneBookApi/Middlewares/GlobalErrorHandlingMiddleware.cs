using System.Data;
using System.Net;
using System.Text.Json;

namespace PhoneBookApi.Middlewares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalErrorHandlingMiddleware(RequestDelegate next) 
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {   
            HttpStatusCode status;
            string message;
            string stackTrace;
            var exceptionType = exception.GetType();
            
            if (exceptionType == typeof(DBConcurrencyException))
            {
                status = HttpStatusCode.BadRequest;
                message = exception.Message;
                stackTrace = exception.StackTrace!;
            }
            else 
            {
                status = HttpStatusCode.InternalServerError;
                message = exception.Message;
                stackTrace = exception.StackTrace!;
            }


            var response = JsonSerializer.Serialize(new { status, message, stackTrace });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            return context.Response.WriteAsync(response);
        }
    }
}