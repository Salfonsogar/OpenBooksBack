using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OpenBooks.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
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

                await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
            }
            catch (InvalidOperationException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.ContentType = "application/json";

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Title = "Operación no autorizada",
                    Detail = ex.Message
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Error interno del servidor",
                    Detail = ex.Message
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
            }
        }
    }
}
