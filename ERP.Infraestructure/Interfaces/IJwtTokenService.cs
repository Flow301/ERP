namespace ERP.Infraestructure.Interfaces;

public interface IJwtTokenService
{
    (string Token, DateTime ExpiresAt) CreateToken(int idUser, string login, string fullName, string role);
}
