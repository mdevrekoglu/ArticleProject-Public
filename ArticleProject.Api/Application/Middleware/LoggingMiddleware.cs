using ArticleProject.Api.Attributes;
using ArticleProject.Domain.Aggregates;
using ArticleProject.Domain.Common.Interfaces.Repository;
using System.Security.Claims;
using System.Text.Json;

namespace ArticleProject.Api.Application.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ILogRepository logRepository)
        {
            context.Request.EnableBuffering();

            var endpoint = context.GetEndpoint();
            var logRequestAttribute = endpoint?.Metadata.GetMetadata<LogRequestAttribute>();

            if (logRequestAttribute != null)
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

                var logEntry = new LogEntry
                {
                    UserId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                    IpAddress = context.Connection.RemoteIpAddress?.ToString(),
                    RequestPath = context.Request.Path,
                    HttpMethod = context.Request.Method,
                    QueryString = context.Request.QueryString.ToString(),
                    RequestBody = requestBody,
                    StackTrace = string.Empty,
                    IsError = false
                };

                await logRepository.AddLogAsync(logEntry);
            }
            await _next(context);
        }

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
            var sensitiveEndpoints = new[] { "/api/authentication/login", "/api/authentication/register", "/api/authentication/confirmemail",
                                            "/api/authentication/resetpassword", "/api/authentication/changepassword" };
            return sensitiveEndpoints.Contains(path.ToLower());
        }
    }
}