using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MushroomB2B.Application;
using MushroomB2B.Domain.Exceptions;
using MushroomB2B.Infrastructure;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

// ─── Layer DI Registration ──────────────────────────────────────────────────
builder.Services.AddApplication();                          // MediatR + Validators
builder.Services.AddInfrastructure(builder.Configuration);  // EF Core + Services

// ─── API ────────────────────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "MushroomB2B API", Version = "v1" });
});

// ─── Global Exception Handling ──────────────────────────────────────────────
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();


// ─── Global Exception Handler (inline for brevity) ─────────────────────────
public sealed class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, title) = exception switch
        {
            ValidationException ve => (StatusCodes.Status422UnprocessableEntity,
                string.Join("; ", ve.Errors.Select(e => e.ErrorMessage))),
            DomainException de => (StatusCodes.Status400BadRequest, de.Message),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
        };

        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Instance = context.Request.Path
        }, cancellationToken);

        return true;
    }
}
