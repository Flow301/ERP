using ERP.Domain.DTO.PatternResult;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace ERP.WebAPI.Handlers;
/// <summary>
/// Manejador global: atrapa toda excepción que nadie manejó, la registra con ILogger
/// y responde con el mismo formato Result&lt;T&gt; que usa el resto de la API.
/// </summary>
public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }
    public async ValueTask<bool> TryHandleAsync(
    HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        // 1. Código de seguimiento: el mismo TraceId que ASP.NET Core y Serilog ponen en el log
        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;
        var user = httpContext.User.Identity?.Name ?? "anónimo";
        // 2. Tipo de excepción → código HTTP + mensaje SEGURO para el cliente
        var (status, title, detail) = exception switch
        {
            // La petición HTTP venía mal formada (cuerpo demasiado grande, encabezado inválidos...)
            BadHttpRequestException badRequest =>
            (badRequest.StatusCode, "Bad Request", "La solicitud no es válida."),
            // SQL Server rechazó el cambio: FK (547), índice único (2601) o llave primaria (2627)
            DbUpdateException or SqlException { Number: 547 or 2601 or 2627 } =>
            (StatusCodes.Status409Conflict, "Conflict",
            "No se pudo guardar porque el registro entra en conflicto con datos existentes."),
            // Cualquier otra cosa es un error del servidor
            _ => (StatusCodes.Status500InternalServerError, "Internal Server Error",
            "Ocurrió un error inesperado.")
        };
        // 3. Log estructurado: la excepción COMPLETA va al log, nunca al cliente
        if (status >= StatusCodes.Status500InternalServerError)
            _logger.LogError(exception,
            "Error no controlado {TraceId} en {Method} {Path}. Usuario: {User}",
            traceId, httpContext.Request.Method, httpContext.Request.Path, user);
        else
            _logger.LogWarning(exception,
            "Excepción convertida en {StatusCode} {TraceId} en {Method} {Path}. Usuario:{ User} ",
 status, traceId, httpContext.Request.Method, httpContext.Request.Path, user);
        // 4. Respuesta estándar del curso: Result<T> con IsSuccess = false
        var result = Result<object>.Failure(status, title,
        $"{detail} Si el problema persiste, reporte el código {traceId}.");
        httpContext.Response.StatusCode = status;
        await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);
        return true; // manejada: ASP.NET Core no sigue buscando otro manejador
    }
}