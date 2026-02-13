using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Acceloka.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

            var problemDetails = new ProblemDetails
            {
                Instance = httpContext.Request.Path
            };

            // Mapping Exception ke HTTP Status Code
            switch (exception)
            {
                case ValidationException validationEx:
                    problemDetails.Title = "Validation Error";
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Detail = "One or more validation errors occurred.";
                    problemDetails.Extensions["errors"] = validationEx.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
                    break;

                case KeyNotFoundException:
                    problemDetails.Title = "Not Found";
                    problemDetails.Status = StatusCodes.Status404NotFound;
                    problemDetails.Detail = exception.Message;
                    break;

                case InvalidOperationException: // Business Logic Error (Quota, Date, etc)
                    problemDetails.Title = "Business Logic Error";
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Detail = exception.Message;
                    break;

                case ArgumentException: // Bad Input
                    problemDetails.Title = "Bad Request";
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Detail = exception.Message;
                    break;

                default:
                    problemDetails.Title = "Internal Server Error";
                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                    problemDetails.Detail = exception.Message;
                    break;
            }

            httpContext.Response.StatusCode = problemDetails.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}