using ERP.Application.Interfaces;
using ERP.Domain.DTO;
using ERP.Domain.DTO.PatternResult;
using ERP.Domain.Entities;
using ERP.Infraestructure.Interfaces;
using MapsterMapper;

namespace ERP.Application.Implementation;

public class SupplierService : ISupplierService
{
    // Regla de paginación: evita que un cliente pida miles de registros de una vez
    public const int MaxPageSize = 50;

    private readonly ISupplierRepository _repository;
    private readonly IMapper _mapper;

    public SupplierService(ISupplierRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    // =====================================================================
    // READ
    // =====================================================================

    public async Task<Result<IEnumerable<SupplierListDto>>> GetAllAsync()
    {
        var suppliers = await _repository.GetAllAsync();

        var data = _mapper.Map<IEnumerable<SupplierListDto>>(suppliers);

        var result = Result<IEnumerable<SupplierListDto>>.Success(data);

        return result;
    }

    public async Task<Result<IEnumerable<SupplierListDto>>> GetPaginationAsync(int pageNumber, int pageSize)
    {
        if (pageNumber < 1 || pageSize < 1 || pageSize > MaxPageSize)
            return Result<IEnumerable<SupplierListDto>>.Failure(400, "Bad Request",
                $"pageNumber must be 1 or more and pageSize between 1 and {MaxPageSize}!");

        var suppliers = await _repository.GetPaginationAsync(pageNumber, pageSize);
        var totalRecords = await _repository.CountAsync();

        var data = _mapper.Map<IEnumerable<SupplierListDto>>(suppliers);

        var result = Result<IEnumerable<SupplierListDto>>.Paged(data, pageNumber, pageSize, totalRecords);

        return result;
    }

    public async Task<Result<SupplierDetailDto>> GetByIdAsync(int id)
    {
        var supplier = await _repository.GetByIdAsync(id);

        if (supplier is null)
            return Result<SupplierDetailDto>.Failure(404, "Not Found", $"Supplier {id} doesn't exist!");

        var data = _mapper.Map<SupplierDetailDto>(supplier);

        var result = Result<SupplierDetailDto>.Success(data);

        return result;
    }

    // =====================================================================
    // CREATE
    // =====================================================================

    public async Task<Result<SupplierDetailDto>> AddAsync(SupplierCreateDto supplier)
    {
        // DTO de entrada → entidad
        var entity = _mapper.Map<Supplier>(supplier);
        entity.Status = true;                      // Regla de negocio: todo proveedor nuevo nace activo

        await _repository.AddAsync(entity);        // el repositorio asigna IdSupplier

        // Entidad → DTO de salida
        var data = _mapper.Map<SupplierDetailDto>(entity);

        var result = Result<SupplierDetailDto>.Success(data, 201, "Created", "Record saved!");

        return result;
    }

    // =====================================================================
    // UPDATE
    // =====================================================================

    public async Task<Result<SupplierDetailDto>> UpdateAsync(int id, SupplierUpdateDto supplier)
    {
        if (!await _repository.ExistsAsync(id))
            return Result<SupplierDetailDto>.Failure(404, "Not Found", $"Supplier {id} doesn't exist!");

        var entity = _mapper.Map<Supplier>(supplier);
        entity.IdSupplier = id;                    // el Id viene de la RUTA, nunca del body

        await _repository.UpdateAsync(entity);

        var data = _mapper.Map<SupplierDetailDto>(entity);

        var result = Result<SupplierDetailDto>.Success(data, 200, "OK", "Record updated!");

        return result;
    }

    // =====================================================================
    // DELETE
    // =====================================================================

    public async Task<Result<int>> DeleteAsync(int id)
    {
        if (!await _repository.ExistsAsync(id))
            return Result<int>.Failure(404, "Not Found", $"Supplier {id} doesn't exist!");

        // Regla de negocio: no se elimina un proveedor que tiene productos asociados
        if (await _repository.HasProductsAsync(id))
            return Result<int>.Failure(409, "Conflict", $"Supplier {id} has products and can't be deleted!");

        await _repository.DeleteAsync(id);

        var result = Result<int>.Success(id, 200, "OK", "Record deleted!");

        return result;
    }
}
