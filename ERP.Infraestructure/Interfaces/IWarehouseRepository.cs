using ERP.Domain.Entities;

namespace ERP.Infraestructure.Interfaces;

public interface IWarehouseRepository
{
    Task<IEnumerable<Warehouse>> GetAllAsync();

    Task<IEnumerable<Warehouse>> GetPaginationAsync(
        int pageNumber,
        int pageSize);

    Task<int> CountAsync();

    Task<Warehouse?> GetByIdAsync(int id);

    Task<bool> ExistsAsync(int id);
    Task<bool> ExistsByNameAsync(string name, int? excludeId = null);

    Task<bool> HasProductsAsync(int id);

    Task AddAsync(Warehouse warehouse);

    Task UpdateAsync(Warehouse warehouse);

    Task DeleteAsync(int id);
}
