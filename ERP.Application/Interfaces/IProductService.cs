using ERP.Domain.DTO;
using ERP.Domain.DTO.PatternResult;
namespace ERP.Application.Interfaces;

public interface IProductService
{
    // ----- Laboratorios de LINQ (6-1: ahora responden con Result<T>) -----
    Task<Result<IEnumerable<ProductListDto>>> GetAllAsync();
    Task<Result<IEnumerable<ProductListDto>>> GetAllOrderDescAsync();
    Task<Result<IEnumerable<ProductListDto>>> FindByNameAsync(string name);
    Task<Result<ProductDetailDto>> GetByIdAsync(int id);
    Task<Result<IEnumerable<ProductListDto>>> GetPaginationAsync(int pageNumber, int
   pageSize);
    Task<Result<IEnumerable<ProductListDto>>> GetPriceAsync(decimal price);
    Task<Result<IEnumerable<ProductListDto>>> GetTop10PriceAsync();
    Task<Result<int>> CountAsync();
    Task<Result<bool>> ExistsByDescriptionAsync(string description);
    Task<Result<IEnumerable<ProductListDto>>> GetByCategoryAsync(int idCategory);
    Task<Result<IEnumerable<ProductListDto>>> GetByCategoryNameAsync(string name);
    Task<Result<IEnumerable<ProductListDto>>> GetByWarehouseIdAsync(int idWarehouse);
    Task<Result<IEnumerable<ProductListDto>>> GetByWarehouseNameAsync(string name);
    Task<Result<IEnumerable<CategoryStatsDto>>> GetStatsByCategoryAsync();
    // ----- NUEVO: CRUD con relaciones -----
    Task<Result<ProductDetailDto>> AddAsync(ProductCreateDto product);
    Task<Result<ProductDetailDto>> UpdateAsync(int id, ProductUpdateDto product);
    Task<Result<int>> DeleteAsync(int id);
    Task<Result<IEnumerable<ProductListDto>>> SearchAsync(ProductSearchDto search);

}