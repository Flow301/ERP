using System.Security.Claims;
using System.Text;
using ERP.Infraestructure.Interfaces;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
namespace ERP.Infraestructure.Implementation;
/// <summary>
/// Genera el JWT firmado con HMAC-SHA256. La llave, el emisor y la audiencia vienen de la configuración.
/// </summary>
public class JwtTokenService : IJwtTokenService
{
    private readonly SigningCredentials _credentials;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expirationMinutes;
    public JwtTokenService(string key, string issuer, string audience, int expirationMinutes)
    {
        // HS256 exige una llave de al menos 256 bits (32 bytes)
        var keyBytes = Encoding.UTF8.GetBytes(key);
        if (keyBytes.Length < 32)
            throw new InvalidOperationException("Jwt:Key debe tener al menos 32 caracteres.");
        _credentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes),
       SecurityAlgorithms.HmacSha256);
        _issuer = issuer;
        _audience = audience;
        _expirationMinutes = expirationMinutes;
    }
    public (string Token, DateTime ExpiresAt) CreateToken(int idUser, string login, string fullName, string role)
    {
        var expiresAt = DateTime.UtcNow.AddMinutes(_expirationMinutes);
        // Claims: solo lo necesario para identificar al usuario. Nunca la contraseña ni datos sensibles:
        // el payload de un JWT se puede leer, solo está firmado, no cifrado.
        var claims = new List<Claim>
        {
        new(JwtRegisteredClaimNames.Sub, idUser.ToString()), // a quién pertenece el token
        new(JwtRegisteredClaimNames.Name, login), // usuario
        new("fullName", fullName),
        new("role", role), // la siguiente clase lo usa para autorizar
        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // identificador único del token
        };
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _issuer,
            Audience = _audience,
            Expires = expiresAt,
            SigningCredentials = _credentials
        };
        var token = new JsonWebTokenHandler().CreateToken(descriptor);
        return (token, expiresAt);
    }
}

