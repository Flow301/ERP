using System.ComponentModel.DataAnnotations;
namespace ERP.Domain.DTO;
/// <summary>
/// Credenciales que envía el usuario para iniciar sesión.
/// </summary>
public record class LoginRequestDto
{
    /// <summary>Nombre de usuario.</summary>
    /// <example>vendedor1</example>
    [Required(ErrorMessage = "El usuario es obligatorio.")]
    [StringLength(50, ErrorMessage = "El usuario no puede superar 50 caracteres.")]
    public string? Login { get; set; }
    /// <summary>Contraseña en texto plano: viaja solo por HTTPS y nunca se guarda ni se registra.</summary>
    /// <example>Vendedor123*</example>
    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(100, ErrorMessage = "La contraseña no puede superar 100 caracteres.")]
    public string? Password { get; set; }
}

