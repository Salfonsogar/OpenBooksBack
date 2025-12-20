using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace OpenBooks.Api.Middlewares
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
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";

                var problemDetails = new ValidationProblemDetails(
                    ex.Errors
                      .GroupBy(e => e.PropertyName)
                      .ToDictionary(
                          g => g.Key,
                          g => g.Select(e => e.ErrorMessage).ToArray()
                      )
                )
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Errores de validación",
                    Detail = "Revisar los campos para más información"
                };

                problemDetails.Extensions["traceId"] = context.TraceIdentifier;

                await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access for request {Path} - TraceId: {TraceId}", context.Request.Path, context.TraceIdentifier);

                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.ContentType = "application/json";

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Title = "No autorizado",
                    Detail = ex.Message
                };

                problemDetails.Extensions["traceId"] = context.TraceIdentifier;

                await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "InvalidOperationException en request {Path} - TraceId: {TraceId}", context.Request.Path, context.TraceIdentifier);

                var inner = ex;
                var isTransient = false;
                while (inner != null)
                {
                    var typeName = inner.GetType().FullName ?? string.Empty;
                    if (typeName.Contains("Npgsql") || inner is SocketException || inner.GetType().Name.Contains("SocketException"))
                    {
                        isTransient = true;
                        break;
                    }
                    inner = (InvalidOperationException)inner.InnerException;
                }

                if (ex.Message?.IndexOf("autoriz", System.StringComparison.OrdinalIgnoreCase) >= 0 ||
                    ex.Message?.IndexOf("no autorizado", System.StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Response.ContentType = "application/json";

                    var problemDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status401Unauthorized,
                        Title = "Operación no autorizada",
                        Detail = ex.Message
                    };

                    problemDetails.Extensions["traceId"] = context.TraceIdentifier;

                    await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
                    return;
                }

                if (isTransient)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                    context.Response.ContentType = "application/json";

                    var problemDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status503ServiceUnavailable,
                        Title = "Servicio no disponible",
                        Detail = "Error transitorio al acceder a recursos externos. Inténtelo de nuevo más tarde."
                    };

                    problemDetails.Extensions["traceId"] = context.TraceIdentifier;

                    await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.Response.ContentType = "application/json";

                    var problemDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Operación inválida",
                        Detail = ex.Message
                    };

                    problemDetails.Extensions["traceId"] = context.TraceIdentifier;

                    await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception for request {Path} - TraceId: {TraceId}", context.Request.Path, context.TraceIdentifier);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Error interno del servidor",
#if DEBUG
                    Detail = ex.Message 
#else
                    Detail = "Se ha producido un error interno. Contacte con soporte con el identificador."
#endif
                };

                problemDetails.Extensions["traceId"] = context.TraceIdentifier;

                await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
            }
        }
    }
}
