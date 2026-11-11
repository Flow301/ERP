using ERP.Domain.Entities;
namespace ERP.Infraestructure.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByLoginAsync(string login);
    Task<string?> GetRoleNameAsync(int idRole);
}
