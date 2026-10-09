using ERP.Domain.DTO;
    namespace ERP.Application.Interfaces;

public interface IProductService
{
    // ----- Laboratorio anterior (adaptados: las listas ahora devuelven ProductListDto) -----
    Task<IEnumerable<ProductListDto>> GetAllAsync();
    Task<IEnumerable<ProductListDto>> GetAllOrderDescAsync();
    Task<IEnumerable<ProductListDto>> FindByNameAsync(string name);
    Task<ProductDetailDto?> GetByIdAsync(int id);
    Task<IEnumerable<ProductListDto>> GetPaginationAsync(int pageNumber, int pageSize);
    Task<IEnumerable<ProductListDto>> GetPriceAsync(decimal price);
    Task<IEnumerable<ProductListDto>> GetTop10PriceAsync();
    Task<int> CountAsync();
    Task<bool> ExistsByDescriptionAsync(string description);
    Task<IEnumerable<ProductListDto>> GetByCategoryAsync(int idCategory);
    // ----- NUEVO: LINQ avanzado -----
    Task<IEnumerable<ProductListDto>> GetByCategoryNameAsync(string name);
    Task<IEnumerable<ProductListDto>> GetByWarehouseIdAsync(int idWarehouse);
    Task<IEnumerable<ProductListDto>> GetByWarehouseNameAsync(string name);
    Task<IEnumerable<CategoryStatsDto>> GetStatsByCategoryAsync();
}
