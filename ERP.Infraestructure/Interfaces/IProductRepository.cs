using ERP.Domain.DTO;
using ERP.Domain.Entities;
namespace ERP.Infraestructure.Interfaces;

public interface IProductRepository
{
    // ----- Laboratorio anterior (ahora con sus relaciones: Include) -----
    Task<IEnumerable<Product>> GetAllAsync();
    Task<IEnumerable<Product>> GetAllOrderDescAsync();
    Task<IEnumerable<Product>> FindByNameAsync(string name);
    Task<Product?> GetByIdAsync(int id);
    Task<IEnumerable<Product>> GetPaginationAsync(int pageNumber, int pageSize);
    Task<IEnumerable<Product>> GetPriceAsync(decimal price);
    Task<IEnumerable<Product>> GetTop10PriceAsync();
    Task<int> CountAsync();
    Task<bool> ExistsByDescriptionAsync(string description); // reto A
    Task<IEnumerable<Product>> GetByCategoryAsync(int idCategory); // reto B
                                                                   // ----- NUEVO: LINQ avanzado -----
    Task<IEnumerable<Product>> GetByCategoryNameAsync(string name);
    Task<IEnumerable<Product>> GetByWarehouseIdAsync(int idWarehouse);
    Task<IEnumerable<Product>> GetByWarehouseNameAsync(string name);
    // Excepción: una agregación (GroupBy) no es una entidad, devuelve un DTO de resultado
    Task<IEnumerable<CategoryStatsDto>> GetStatsByCategoryAsync();
}
