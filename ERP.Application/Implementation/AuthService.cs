using ERP.Application.Interfaces;
using ERP.Domain.DTO;
using ERP.Domain.DTO.PatternResult;
using ERP.Infraestructure.Interfaces;
using MapsterMapper;
using Microsoft.Extensions.Logging;
namespace ERP.Application.Implementation;

public class AuthService : IAuthService
{
    // Hash de una contraseña que nadie conoce. Se compara cuando el login no existe para que la
    // respuesta tarde lo mismo: así el tiempo no revela qué usuarios existen.
    private static readonly string DummyHash = BCrypt.Net.BCrypt.HashPassword("usuarioinexistente", workFactor: 12);
    private readonly IUserRepository _repository;
    private readonly IJwtTokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly ILogger<AuthService> _logger;
    public AuthService(IUserRepository repository, IJwtTokenService tokenService, IMapper mapper, ILogger<AuthService> logger)
    {
        _repository = repository;
        _tokenService = tokenService;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto login)
    {
        var userLogin = login.Login!.Trim();
        var user = await _repository.GetByLoginAsync(userLogin);
        // BCrypt lee el salt y el costo guardados dentro del hash y vuelve a calcularlo con la contraseña recibida
    var validPassword = VerifyPassword(login.Password!, user?.Password ?? DummyHash);
        // Mismo mensaje para "no existe", "contraseña incorrecta" e "inactivo": no se da ninguna pista
    if (user is null || !validPassword || !user.Status)
    {
        _logger.LogWarning("Login fallido para el usuario {Login}", userLogin);
        return Result<LoginResponseDto>.Failure(401, "Unauthorized", "Usuario o contraseña incorrectos.");
    }
        var role = await _repository.GetRoleNameAsync(user.IdRole) ?? string.Empty;
        var (token, expiresAt) = _tokenService.CreateToken(user.IdUser, user.Login, user.FullName, role);
        var data = _mapper.Map<LoginResponseDto>(user); // IdUser, Login, FullName y Email(mismo nombre)
        data.Token = token;
        data.TokenType = "Bearer";
        data.ExpiresAt = expiresAt;
        data.Role = role;
        _logger.LogInformation("Login correcto de {Login} con rol {Role}", user.Login, role);
        var result = Result<LoginResponseDto>.Success(data, 200, "OK", "Login successful!");
        return result;
    }
     // Un hash que no es BCrypt (por ejemplo, de una base vieja) no debe provocar un error 500
    private bool VerifyPassword(string password, string hash)
    {
        try
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
        catch (BCrypt.Net.SaltParseException)
        {
            _logger.LogWarning("El hash guardado no es BCrypt; recree la base para aplicar el Seeder de usuarios");
        return false;
        }
    }
}
