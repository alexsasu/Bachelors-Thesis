using ApplicationCoreLibrary.Exceptions;
using System.Net;
using System.Text.Json;

namespace bachelor_project_api.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                logger.LogError("Message: " + ex.Message + "InnerException: " + ex.InnerException, "Exception middleware caught an exception.");

                var response = context.Response;
                response.ContentType = "application/json";

                switch (ex)
                {
                    case InvalidUrlAnalysisGetRequestException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case InvalidUrlReportGetRequestException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case UrlReportWithGivenIdNotFoundException:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case UrlReportOfGivenUserNotFoundException:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case FailedToRegisterUserException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case UserAlreadyRegisteredException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case HadTroubleProcessingLoginCredentialsException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case UserWithGivenIdNotFoundException:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case UserWithGivenEmailNotFoundException:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case UnauthorizedAccessException:
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;
                    case KeyNotFoundException:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case NotImplementedException:
                        response.StatusCode = (int)HttpStatusCode.NotImplemented;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        logger.LogCritical("Message: " + ex.Message + "InnerException: " + ex.InnerException, "Unexpected error caught by the exception middleware.");
                        break;
                }

                var result = JsonSerializer.Serialize(new { message = ex?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}
