using FluentValidation;
using PredictionEngineApi.Dtos;

namespace PredictionEngineApi.Utils
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _request;
        private readonly ILogger _logger;

        public ExceptionMiddleware(RequestDelegate request, ILogger logger)
        {
            _request = request;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _request(context);
            }
            catch (ValidationException exception)
            {
                context.Response.StatusCode = 400;
                var messages = exception.Errors.Select(x => x.ErrorMessage).ToList();
                var validationFailureResponse = new ValidationFailureResponse
                {
                    Errors = messages
                };
                await context.Response.WriteAsJsonAsync(validationFailureResponse);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);

                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync("Internal server error");
            }
        }
    }
}