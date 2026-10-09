using ERP.Domain.DTO;
using ERP.Domain.DTO.PatternResult;
namespace ERP.Application.Interfaces;

public interface IWarehouseService
{
    Task<Result<IEnumerable<WarehouseDto>>> GetAllAsync();
    Task<Result<IEnumerable<WarehouseDto>>> GetPaginationAsync(int pageNumber, int pageSize);
    Task<Result<WarehouseDto>> GetByIdAsync(int id);
    Task<Result<WarehouseDto>> AddAsync(WarehouseCreateDto warehouse);
    Task<Result<WarehouseDto>> UpdateAsync(int id, WarehouseUpdateDto warehouse);
    Task<Result<int>> DeleteAsync(int id);
}
