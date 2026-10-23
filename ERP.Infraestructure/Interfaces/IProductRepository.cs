using ERP.Domain.DTO;
using ERP.Domain.Entities;
namespace ERP.Infraestructure.Interfaces;

public interface IProductRepository
{
    // ----- Lectura (laboratorios de LINQ): solo productos activos -----
    Task<IEnumerable<Product>> GetAllAsync();
    Task<IEnumerable<Product>> GetAllOrderDescAsync();
    Task<IEnumerable<Product>> FindByNameAsync(string name);
    Task<Product?> GetByIdAsync(int id);
    Task<IEnumerable<Product>> GetPaginationAsync(int pageNumber, int pageSize);
    Task<IEnumerable<Product>> GetPriceAsync(decimal price);
    Task<IEnumerable<Product>> GetTop10PriceAsync();
    Task<int> CountAsync();
    Task<bool> ExistsByDescriptionAsync(string description);
    Task<IEnumerable<Product>> GetByCategoryAsync(int idCategory);
    Task<IEnumerable<Product>> GetByCategoryNameAsync(string name);
    Task<IEnumerable<Product>> GetByWarehouseIdAsync(int idWarehouse);
    Task<IEnumerable<Product>> GetByWarehouseNameAsync(string name);
    Task<IEnumerable<CategoryStatsDto>> GetStatsByCategoryAsync();
    //Recibe el DTO de búsqueda y devuelve la página + el total de registros del filtro
    Task<(IEnumerable<Product> Items, int TotalRecords)> SearchAsync(ProductSearchDto
   search);
    // ----- NUEVO: verificaciones de las relaciones -----
    Task<bool> CategoryExistsAsync(int idCategory);
    Task<bool> WarehouseExistsAsync(int idWarehouse);
    Task<IEnumerable<int>> GetActiveSupplierIdsAsync(IEnumerable<int> supplierIds);
    // ----- NUEVO: escritura (entidades RASTREADAS por el ChangeTracker) -----
    Task<Product?> GetForUpdateAsync(int id); // con Include(ProductSuppliers)
    Task<Product?> GetForDeleteAsync(int id); // sin Include
    Task AddAsync(Product product);
    void RemoveSupplierLinks(IEnumerable<ProductSupplier> links);
    Task SaveChangesAsync();

    Task<bool> ExistsByNameAsync(string productName, int? excludeId = null);



}