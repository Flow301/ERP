using ERP.Application.Interfaces;
using ERP.Domain.DTO;
using ERP.Domain.DTO.PatternResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
namespace ERP.WebAPI.Controllers;
/// <summary>
/// Autenticación: inicio de sesión y lectura del usuario autenticado.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
[ProducesResponseType<Result<object>>(500)]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;
    public AuthController(IAuthService service)
    {
        _service = service;
    }
    /// <summary>
    /// Inicia sesión y devuelve un JWT con los datos básicos del usuario y su rol.
    /// </summary>
    /// <remarks>
    /// Usuarios de prueba: admin (contraseña del secreto Seed:AdminPassword), vendedor1 vendedor2 y vendedor3(Vendedor123*).
    /// El token se envía después en el header Authorization: Bearer {token}.
    /// </remarks>
    /// <param name="login">Usuario y contraseña.</param>
    /// <response code="200">Login correcto: token, vencimiento, usuario y rol.</response>
    /// <response code="400">Faltan el usuario o la contraseña.</response>
    /// <response code="401">Usuario o contraseña incorrectos, o usuario inactivo.</response>
    /// <response code="429">Más de 5 intentos de login en un minuto.</response>
    [HttpPost("login")]
    [AllowAnonymous]                                    // sin token, es la puerta de entrada
    [EnableRateLimiting("login")] // RETO 9-1: máximo 5 intentos por minuto
    [ProducesResponseType<Result<LoginResponseDto>>(200)]
    [ProducesResponseType<Result<Dictionary<string, string[]>>>(400)]
    [ProducesResponseType<Result<object>>(401)]
    [ProducesResponseType<Result<object>>(429)]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDto login)
    {
        var result = await _service.LoginAsync(login);
        return StatusCode(result.Status, result);
    }
    /// <summary>
    /// Devuelve el usuario que viene dentro del token enviado en el header Authorization.
    /// </summary>
    /// <response code="200">Token válido: datos leídos de sus claims.</response>
    /// <response code="401">No se envió token, o es inválido o está vencido.</response>
    [HttpGet("me")]
    [Authorize]                                         // reemplaza el if
    [ProducesResponseType<Result<CurrentUserDto>>(200)]
    [ProducesResponseType<Result<object>>(401)]
    public IActionResult Me()
    {
        var data = new CurrentUserDto
        {
            IdUser = User.FindFirst("sub")?.Value,
            Login = User.FindFirst("name")?.Value,
            FullName = User.FindFirst("fullName")?.Value,
            Role = User.FindFirst("role")?.Value
        };
        var result = Result<CurrentUserDto>.Success(data);
        return StatusCode(result.Status, result);
    }
}