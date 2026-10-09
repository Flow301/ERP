using ERP.Application.Interfaces;
using ERP.Domain.DTO;
using ERP.Domain.DTO.PatternResult;
using ERP.Domain.Entities;
using ERP.Infraestructure.Interfaces;
using MapsterMapper;

namespace ERP.Application.Implementation;

public class WarehouseService : IWarehouseService
{
    // Regla de paginación: evita que un cliente pida miles de registros de una vez
    public const int MaxPageSize = 50;

    private readonly IWarehouseRepository _repository;
    private readonly IMapper _mapper;

    public WarehouseService(IWarehouseRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    // =====================================================================
    // READ
    // =====================================================================

    public async Task<Result<IEnumerable<WarehouseDto>>> GetAllAsync()
    {
        var warehouses = await _repository.GetAllAsync();

        var data = _mapper.Map<IEnumerable<WarehouseDto>>(warehouses);

        var result = Result<IEnumerable<WarehouseDto>>.Success(data);

        return result;
    }

    public async Task<Result<IEnumerable<WarehouseDto>>> GetPaginationAsync(int pageNumber, int pageSize)
    {
        if (pageNumber < 1 || pageSize < 1 || pageSize > MaxPageSize)
            return Result<IEnumerable<WarehouseDto>>.Failure(400, "Bad Request",
                $"pageNumber must be 1 or more and pageSize between 1 and {MaxPageSize}!");

        var warehouses = await _repository.GetPaginationAsync(pageNumber, pageSize);
        var totalRecords = await _repository.CountAsync();

        var data = _mapper.Map<IEnumerable<WarehouseDto>>(warehouses);

        var result = Result<IEnumerable<WarehouseDto>>.Paged(data, pageNumber, pageSize, totalRecords);

        return result;
    }

    public async Task<Result<WarehouseDto>> GetByIdAsync(int id)
    {
        var warehouse = await _repository.GetByIdAsync(id);

        if (warehouse is null)
            return Result<WarehouseDto>.Failure(404, "Not Found", $"Warehouse {id} doesn't exist!");

        var data = _mapper.Map<WarehouseDto>(warehouse);

        var result = Result<WarehouseDto>.Success(data);

        return result;
    }

    // =====================================================================
    // CREATE
    // =====================================================================

    public async Task<Result<WarehouseDto>> AddAsync(WarehouseCreateDto warehouse)
    {
        var name = warehouse.WarehouseName!.Trim();

        // Regla de negocio: el nombre de la bodega no se repite (también hay un índice UNIQUE en la tabla)
        if (await _repository.ExistsByNameAsync(name))
            return Result<WarehouseDto>.Failure(409, "Conflict", $"Warehouse '{name}' already exists!");

        // DTO de entrada → entidad
        var entity = _mapper.Map<Warehouse>(warehouse);
        entity.WarehouseName = name;

        await _repository.AddAsync(entity);        // el repositorio asigna IdWarehouse

        // Entidad → DTO de salida
        var data = _mapper.Map<WarehouseDto>(entity);

        var result = Result<WarehouseDto>.Success(data, 201, "Created", "Record saved!");

        return result;
    }

    // =====================================================================
    // UPDATE
    // =====================================================================

    public async Task<Result<WarehouseDto>> UpdateAsync(int id, WarehouseUpdateDto warehouse)
    {
        if (!await _repository.ExistsAsync(id))
            return Result<WarehouseDto>.Failure(404, "Not Found", $"Warehouse {id} doesn't exist!");

        var name = warehouse.WarehouseName!.Trim();

        // El nombre no puede ser el de OTRA bodega; conservar el propio sí se permite
        if (await _repository.ExistsByNameAsync(name, id))
            return Result<WarehouseDto>.Failure(409, "Conflict", $"Warehouse '{name}' already exists!");

        var entity = _mapper.Map<Warehouse>(warehouse);
        entity.IdWarehouse = id;                   // el Id viene de la RUTA, nunca del body
        entity.WarehouseName = name;

        await _repository.UpdateAsync(entity);

        var data = _mapper.Map<WarehouseDto>(entity);

        var result = Result<WarehouseDto>.Success(data, 200, "OK", "Record updated!");

        return result;
    }

    // =====================================================================
    // DELETE
    // =====================================================================

    public async Task<Result<int>> DeleteAsync(int id)
    {
        if (!await _repository.ExistsAsync(id))
            return Result<int>.Failure(404, "Not Found", $"Warehouse {id} doesn't exist!");

        // Regla de negocio: no se elimina una bodega que tiene productos (Product.IdWarehouse)
        if (await _repository.HasProductsAsync(id))
            return Result<int>.Failure(409, "Conflict", $"Warehouse {id} has products and can't be deleted!");

        await _repository.DeleteAsync(id);

        var result = Result<int>.Success(id, 200, "OK", "Record deleted!");

        return result;
    }
}
