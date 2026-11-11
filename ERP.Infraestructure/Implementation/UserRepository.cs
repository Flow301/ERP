using ERP.Domain.Entities;
using ERP.Infraestructure.Data;
using ERP.Infraestructure.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace ERP.Infraestructure.Implementation;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    // Busca el usuario por su login. El servicio decide si la contraseña y el estado son válidos.
    public async Task<User?> GetByLoginAsync(string login)
    {
        var result = await _context.Set<User>()
        .AsNoTracking()
        .FirstOrDefaultAsync(u => u.Login == login);
        return result;
    }
    // Nombre del rol para el claim "role" del token: Administrador, Vendedor o Cajero
    public async Task<string?> GetRoleNameAsync(int idRole)
    {
        var result = await _context.Set<Role>()
        .Where(r => r.IdRole == idRole)
        .Select(r => r.RoleName)
        .FirstOrDefaultAsync();
        return result;
    }
}
