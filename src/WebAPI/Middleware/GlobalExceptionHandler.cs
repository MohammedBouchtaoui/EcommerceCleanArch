using Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Middleware;

public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IHostEnvironment env) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context, Exception exception, CancellationToken ct)
    {
        var (status, title) = exception switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, "Ressource introuvable"),
            BadRequestException => (StatusCodes.Status400BadRequest, "Requête invalide"),
            _ => (StatusCodes.Status500InternalServerError, "Erreur interne du serveur")
        };

        if (status >= 500)
            logger.LogError(exception, "Exception non gérée : {Message}", exception.Message);
        else
            logger.LogWarning("{Title} : {Message}", title, exception.Message);

        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            // On n'expose le détail d'une erreur 500 qu'en développement
            Detail = status < 500 || env.IsDevelopment() ? exception.Message : null,
            Instance = context.Request.Path
        };
        problem.Extensions["traceId"] = context.TraceIdentifier;

        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync(problem, ct);
        return true;
    }
}