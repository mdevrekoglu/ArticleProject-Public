using ArticleProject.Domain.Aggregates;
using ArticleProject.Domain.Common.Interfaces.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace ArticleProject.Api.Application.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ILogRepository logRepository)
        {
            try
            {
                await _next(context); // Call the next middleware
            }
            catch (Exception ex)
            {
                // If error is not fluent validation exception log the error
                if (ex is not FluentValidation.ValidationException && ex is not NullReferenceException)
                {
                    string requestBody = string.Empty;

                    if (context.Request.ContentLength > 0)
                    {
                        using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
                        var rawBody = await reader.ReadToEndAsync();
                        context.Request.Body.Position = 0;

                        // Check if the endpoint is sensitive and mask sensitive data
                        requestBody = IsSensitiveEndpoint(context.Request.Path) ? MaskSensitiveData(rawBody) : rawBody;
                    }

                    var errorLog = new LogEntry
                    {
                        UserId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                        IpAddress = context.Connection.RemoteIpAddress?.ToString(),
                        RequestPath = context.Request.Path,
                        HttpMethod = context.Request.Method,
                        QueryString = context.Request.QueryString.ToString(),
                        IsError = true,
                        RequestBody = requestBody,
                        StackTrace = ex.StackTrace
                    };

                    await logRepository.AddLogAsync(errorLog);
                }

                // Return response to user
                var problemDetails = new ProblemDetails();
                problemDetails.Instance = context.Request.Path;
                if (ex is FluentValidation.ValidationException fluentException)
                {
                    problemDetails.Title = "One or more validation errors occurred.";
                    //problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";

                    context.Response.StatusCode = StatusCodes.Status400BadRequest;

                    List<string> validationErrors = new List<string>();

                    foreach (var error in fluentException.Errors)
                    {
                        validationErrors.Add(error.ErrorMessage);
                    }

                    problemDetails.Extensions.Add("errors", validationErrors);
                }
                else if (ex is NullReferenceException)
                {
                    problemDetails.Title = ex.Message;
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                }
                else
                {
                    problemDetails.Title = ex.Message;
                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                }

                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }

        // TODO Loggingde de aynı fonksiyonlar var buradakiler ya silinmeli ya da farklı bir yönteme gidilmeli
        private string MaskSensitiveData(string jsonBody)
        {
            try
            {
                var jsonDocument = JsonDocument.Parse(jsonBody);
                var jsonObject = jsonDocument.RootElement.Clone();

                // Create a dictionary to modify sensitive fields
                var jsonObjectDict = jsonObject.EnumerateObject().ToDictionary(x => x.Name, x => x.Value.ToString());

                // Mask sensitive fields like password
                if (jsonObjectDict.ContainsKey("password"))
                {
                    jsonObjectDict["password"] = "*****";
                }

                if (jsonObjectDict.ContainsKey("newPassword"))
                {
                    jsonObjectDict["newPassword"] = "*****";
                }

                if (jsonObjectDict.ContainsKey("token"))
                {
                    jsonObjectDict["token"] = "token";
                }

                return JsonSerializer.Serialize(jsonObjectDict);
            }
            catch
            {
                // Return the original JSON body if parsing fails
                return jsonBody;
            }
        }

        private bool IsSensitiveEndpoint(string path)
        {
            var sensitiveEndpoints = new[] { "/api/authentication/login", "/api/authentication/register", "/api/authentication/confirmemail"
                                            , "/api/authentication/resetpassword", "/api/authentication/changepassword" };
            return sensitiveEndpoints.Contains(path.ToLower());
        }
    }
}