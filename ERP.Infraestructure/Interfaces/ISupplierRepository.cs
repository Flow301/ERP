using ERP.Domain.Entities;
namespace ERP.Infraestructure.Interfaces;

public interface ISupplierRepository
{
    // Lectura
    Task<IEnumerable<Supplier>> GetAllAsync();
    Task<IEnumerable<Supplier>> GetPaginationAsync(int pageNumber, int pageSize);
    Task<int> CountAsync();
    Task<Supplier?> GetByIdAsync(int id);
    // Verificaciones para las reglas de negocio
    Task<bool> ExistsAsync(int id);
    Task<bool> HasProductsAsync(int id);
    // Escritura
    Task AddAsync(Supplier supplier);
    Task UpdateAsync(Supplier supplier);
    Task DeleteAsync(int id);
}