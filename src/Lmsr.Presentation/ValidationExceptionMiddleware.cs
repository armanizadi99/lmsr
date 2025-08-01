using System.Net;
using System.Text.Json;
using FluentValidation;
namespace Lmsr.Presentation;

public class ValidationExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ValidationExceptionMiddleware(RequestDelegate next)
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

            var errors = ex.Errors
                .Select(err => new { err.PropertyName, err.ErrorMessage });

            var result = JsonSerializer.Serialize(new
            {
                message = "One or more validation errors occurred.",
                errors
            });

            await context.Response.WriteAsync(result);
        }
    }
}
