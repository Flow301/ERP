using ERP.Domain.DTO;
using ERP.Domain.DTO.PatternResult;
namespace ERP.Application.Interfaces;

public interface IProductService
{
    // ----- Laboratorios de LINQ -----
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
    Task<IEnumerable<ProductListDto>> GetByCategoryNameAsync(string name);
    Task<IEnumerable<ProductListDto>> GetByWarehouseIdAsync(int idWarehouse);
    Task<IEnumerable<ProductListDto>> GetByWarehouseNameAsync(string name);
    Task<IEnumerable<CategoryStatsDto>> GetStatsByCategoryAsync();
    // ----- NUEVO: CRUD con relaciones -----
    Task<Result<ProductDetailDto>> AddAsync(ProductCreateDto product);
    Task<Result<ProductDetailDto>> UpdateAsync(int id, ProductUpdateDto product);
    Task<Result<int>> DeleteAsync(int id);
    Task<Result<IEnumerable<ProductListDto>>> SearchAsync(ProductSearchDto search);

}