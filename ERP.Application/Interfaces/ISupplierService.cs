using ERP.Domain.DTO;
using ERP.Domain.DTO.PatternResult;
namespace ERP.Application.Interfaces;

public interface ISupplierService
{
    Task<Result<IEnumerable<SupplierListDto>>> GetAllAsync();
    Task<Result<IEnumerable<SupplierListDto>>> GetPaginationAsync(int pageNumber, int
   pageSize);
    Task<Result<SupplierDetailDto>> GetByIdAsync(int id);
    Task<Result<SupplierDetailDto>> AddAsync(SupplierCreateDto supplier);
    Task<Result<SupplierDetailDto>> UpdateAsync(int id, SupplierUpdateDto supplier);
    Task<Result<int>> DeleteAsync(int id);
}