namespace ERP.Domain.DTO;
/// <summary>
/// Datos del usuario que se leen de los claims del token (GET api/Auth/me).
/// </summary>
public record class CurrentUserDto
{
    public string? IdUser { get; set; }
    public string? Login { get; set; }
    public string? FullName { get; set; }
    public string? Role { get; set; }
}
