namespace ERP.Domain.DTO;
/// <summary>
/// Respuesta de un login correcto: el token y los datos básicos del usuario. Nunca incluye la contraseña.
/// </summary>
public record class LoginResponseDto
{
    public string? Token { get; set; }
    public string? TokenType { get; set; } // "Bearer"
    public DateTime ExpiresAt { get; set; } // fecha y hora UTC en que vence el token
    public int IdUser { get; set; }
    public string? Login { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }
}
