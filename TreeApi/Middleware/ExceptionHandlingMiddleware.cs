using System.Text.Json;
using TreeApi.Exceptions;
using TreeApi.Models;
using TreeApi.Services;

namespace TreeApi.Middleware
{
    public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        /// <summary>
        /// Processes the HTTP request and handles any exceptions that occur
        /// </summary>
        /// <param name="context">The HTTP context for the request</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Handles exceptions by logging them and returning appropriate error responses
        /// </summary>
        /// <param name="context">The HTTP context for the request</param>
        /// <param name="exception">The exception that occurred</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var requestPath = context.Request.Path.Value;
            var httpMethod = context.Request.Method;
            var queryParameters = context.Request.QueryString.Value;
            var bodyParameters = await GetRequestBodyAsync(context.Request);

            var exceptionJournalService = context.RequestServices.GetRequiredService<IExceptionJournalService>();

            var journalEntry = await exceptionJournalService.LogExceptionAsync(
                exception,
                queryParameters,
                bodyParameters,
                requestPath,
                httpMethod);

            ErrorResponse errorResponse;

            if (exception is SecureException)
            {
                errorResponse = new ErrorResponse
                {
                    Type = "Secure",
                    Id = journalEntry.EventId.ToString(),
                    Data = new ErrorData
                    {
                        Message = exception.Message
                    }
                };
            }
            else
            {
                errorResponse = new ErrorResponse
                {
                    Type = "Exception",
                    Id = journalEntry.EventId.ToString(),
                    Data = new ErrorData
                    {
                        Message = $"Internal server error ID = {journalEntry.EventId}"
                    }
                };
            }

            var jsonResponse = JsonSerializer.Serialize(errorResponse, _jsonOptions);

            await context.Response.WriteAsync(jsonResponse);
        }

        /// <summary>
        /// Attempts to read the request body as a string
        /// </summary>
        /// <param name="request">The HTTP request</param>
        /// <returns>The request body as a string, or null if unable to read</returns>
        private async Task<string?> GetRequestBodyAsync(HttpRequest request)
        {
            try
            {
                if (request.Body.CanSeek)
                {
                    request.Body.Seek(0, SeekOrigin.Begin);
                    using var reader = new StreamReader(request.Body);
                    return await reader.ReadToEndAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to read request body");
            }
            return null;
        }
    }
}
