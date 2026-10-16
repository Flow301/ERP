using ERP.Application.Interfaces;
using ERP.Domain.DTO;
using ERP.Infraestructure.Interfaces;
using MapsterMapper;

namespace ERP.Application.Implementation;

using ERP.Domain.DTO.PatternResult;
using ERP.Domain.Entities;


public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    // =====================================================================
    // Laboratorio anterior — el repositorio devuelve entidades;
    // el servicio las convierte en DTOs con el mapper
    // =====================================================================

    public async Task<IEnumerable<ProductListDto>> GetAllAsync()
    {
        var products = await _repository.GetAllAsync();

        var result = _mapper.Map<IEnumerable<ProductListDto>>(products);

        return result;
    }

    public async Task<IEnumerable<ProductListDto>> GetAllOrderDescAsync()
    {
        var products = await _repository.GetAllOrderDescAsync();

        var result = _mapper.Map<IEnumerable<ProductListDto>>(products);

        return result;
    }

    public async Task<IEnumerable<ProductListDto>> FindByNameAsync(string name)
    {
        var products = await _repository.FindByNameAsync(name.Trim());

        var result = _mapper.Map<IEnumerable<ProductListDto>>(products);

        return result;
    }

    // Detalle: el mapper arma los DTOs anidados y los campos calculados
    public async Task<ProductDetailDto?> GetByIdAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);

        if (product is null)
            return null;

        var result = _mapper.Map<ProductDetailDto>(product);

        return result;
    }

    public async Task<IEnumerable<ProductListDto>> GetPaginationAsync(int pageNumber, int pageSize)
    {
        var products = await _repository.GetPaginationAsync(pageNumber, pageSize);

        var result = _mapper.Map<IEnumerable<ProductListDto>>(products);

        return result;
    }

    public async Task<IEnumerable<ProductListDto>> GetPriceAsync(decimal price)
    {
        var products = await _repository.GetPriceAsync(price);

        var result = _mapper.Map<IEnumerable<ProductListDto>>(products);

        return result;
    }

    public async Task<IEnumerable<ProductListDto>> GetTop10PriceAsync()
    {
        var products = await _repository.GetTop10PriceAsync();

        var result = _mapper.Map<IEnumerable<ProductListDto>>(products);

        return result;
    }

    // Un número o un bool no necesitan mapper
    public async Task<int> CountAsync()
    {
        var result = await _repository.CountAsync();

        return result;
    }

    public async Task<bool> ExistsByDescriptionAsync(string description)
    {
        var result = await _repository.ExistsByDescriptionAsync(description.Trim());

        return result;
    }

    public async Task<IEnumerable<ProductListDto>> GetByCategoryAsync(int idCategory)
    {
        var products = await _repository.GetByCategoryAsync(idCategory);

        var result = _mapper.Map<IEnumerable<ProductListDto>>(products);

        return result;
    }

    // =====================================================================
    // NUEVO — LINQ avanzado
    // =====================================================================

    public async Task<IEnumerable<ProductListDto>> GetByCategoryNameAsync(string name)
    {
        var products = await _repository.GetByCategoryNameAsync(name.Trim());

        var result = _mapper.Map<IEnumerable<ProductListDto>>(products);

        return result;
    }

    public async Task<IEnumerable<ProductListDto>> GetByWarehouseIdAsync(int idWarehouse)
    {
        var products = await _repository.GetByWarehouseIdAsync(idWarehouse);

        var result = _mapper.Map<IEnumerable<ProductListDto>>(products);

        return result;
    }

    public async Task<IEnumerable<ProductListDto>> GetByWarehouseNameAsync(string name)
    {
        var products = await _repository.GetByWarehouseNameAsync(name.Trim());

        var result = _mapper.Map<IEnumerable<ProductListDto>>(products);

        return result;
    }

    // Excepción: el GroupBy ya llega como DTO desde el repositorio, no necesita mapper
    public async Task<IEnumerable<CategoryStatsDto>> GetStatsByCategoryAsync()
    {
        var result = await _repository.GetStatsByCategoryAsync();

        return result;
    }
    // =====================================================================
    // NUEVO — CRUD con relaciones
    // =====================================================================

    // CREATE: valida las relaciones, arma el grafo Product + ProductSuppliers y lo guarda junto
    public async Task<Result<ProductDetailDto>> AddAsync(ProductCreateDto product)
    {
        var error = await ValidateRelationsAsync(product.IdCategory, product.IdWarehouse, product.SupplierIds);

        // Regla de negocio: nombre único entre los productos activos
        if (await _repository.ExistsByNameAsync(product.ProductName!.Trim()))
            return Result<ProductDetailDto>.Failure(409, "Conflict",
 $"Ya existe un producto con el nombre '{product.ProductName}'.");

        if (error is not null)
            return Result<ProductDetailDto>.Failure(400, "Bad Request", error);

        // DTO de entrada → entidad (campos simples + IdCategory + IdWarehouse)
        var entity = _mapper.Map<Product>(product);
        entity.InsertDate = DateTime.UtcNow;      // dato de auditoría: lo define el servidor
        entity.Status = true;                     // todo producto nuevo nace activo

        // Relación N:N: una fila de ProductSupplier por cada proveedor (sin repetir)
        foreach (var idSupplier in product.SupplierIds.Distinct())
            entity.ProductSuppliers.Add(new ProductSupplier { IdSupplier = idSupplier });

        await _repository.AddAsync(entity);       // INSERT Product + INSERT ProductSupplier, una transacción

        // Se vuelve a leer con sus relaciones para responder con el detalle completo
        var created = await _repository.GetByIdAsync(entity.IdProduct);

        var data = _mapper.Map<ProductDetailDto>(created!);

        var result = Result<ProductDetailDto>.Success(data, 201, "Created", "Record saved!");

        return result;
    }

    // UPDATE: entidad rastreada + mapeo sobre ella + sincronización de la tabla puente
    public async Task<Result<ProductDetailDto>> UpdateAsync(int id, ProductUpdateDto product)
    {
        var entity = await _repository.GetForUpdateAsync(id);   // rastreada, con Include(ProductSuppliers)

        if (entity is null)
            return Result<ProductDetailDto>.Failure(404, "Not Found", $"Product {id} doesn't exist!");

        var error = await ValidateRelationsAsync(product.IdCategory, product.IdWarehouse, product.SupplierIds);

        // Misma regla, excluyendo al propio producto
        if (await _repository.ExistsByNameAsync(product.ProductName!.Trim(), id))
            return Result<ProductDetailDto>.Failure(409, "Conflict",
            $"Ya existe un producto con el nombre '{product.ProductName}'.");

        if (error is not null)
            return Result<ProductDetailDto>.Failure(400, "Bad Request", error);

        // Copia los valores del DTO SOBRE la entidad rastreada:
        // el ChangeTracker compara con su snapshot y marca solo las columnas que cambiaron
        _mapper.Map(product, entity);

        SyncSuppliers(entity, product.SupplierIds);

        await _repository.SaveChangesAsync();     // UPDATE + INSERT/DELETE en ProductSupplier, una transacción

        var updated = await _repository.GetByIdAsync(id);

        var data = _mapper.Map<ProductDetailDto>(updated!);

        var result = Result<ProductDetailDto>.Success(data, 200, "OK", "Record updated!");

        return result;
    }

    // DELETE lógico: Status = false. Conserva el historial (facturas) y la integridad referencial
    public async Task<Result<int>> DeleteAsync(int id)
    {
        var entity = await _repository.GetForDeleteAsync(id);

        if (entity is null)
            return Result<int>.Failure(404, "Not Found", $"Product {id} doesn't exist!");

        entity.Status = false;                    // ChangeTracker: Modified, solo la columna Status

        await _repository.SaveChangesAsync();     // UPDATE Product SET Status = 0 WHERE IdProduct = @id

        var result = Result<int>.Success(id, 200, "OK", "Record deleted!");

        return result;
    }

    // ---------------------------------------------------------------------
    // Reglas de negocio de las relaciones: los Ids recibidos deben existir
    // Devuelve null si todo está bien, o el mensaje de error
    // ---------------------------------------------------------------------
    private async Task<string?> ValidateRelationsAsync(int idCategory, int idWarehouse, IEnumerable<int> supplierIds)
    {
        if (!await _repository.CategoryExistsAsync(idCategory))
            return $"Category {idCategory} doesn't exist!";

        if (!await _repository.WarehouseExistsAsync(idWarehouse))
            return $"Warehouse {idWarehouse} doesn't exist!";

        var requested = supplierIds.Distinct().ToList();
        var active = await _repository.GetActiveSupplierIdsAsync(requested);
        var invalid = requested.Except(active).ToList();

        if (invalid.Any())
            return $"Suppliers {string.Join(", ", invalid)} don't exist or are inactive!";

        return null;
    }

    // ---------------------------------------------------------------------
    // Sincronización N:N: compara los Ids recibidos con los que ya tiene el producto
    // ---------------------------------------------------------------------
    private void SyncSuppliers(Product entity, IEnumerable<int> supplierIds)
    {
        var requested = supplierIds.Distinct().ToList();
        var current = entity.ProductSuppliers.Select(ps => ps.IdSupplier).ToList();

        // 1. Quitar los que ya no vienen → DELETE en ProductSupplier
        var toRemove = entity.ProductSuppliers
            .Where(ps => !requested.Contains(ps.IdSupplier))
            .ToList();

        _repository.RemoveSupplierLinks(toRemove);

        // 2. Agregar los que no existían → INSERT en ProductSupplier
        foreach (var idSupplier in requested.Except(current))
            entity.ProductSuppliers.Add(new ProductSupplier { IdProduct = entity.IdProduct, IdSupplier = idSupplier });

        // 3. Los que siguen en la lista quedan Unchanged: no se toca nada
    }

    // Búsqueda: el DTO ya llega validado (atributos + IValidatableObject)
    public async Task<Result<IEnumerable<ProductListDto>>> SearchAsync(ProductSearchDto
   search)
    {
        var products = await _repository.SearchAsync(
        search.Name?.Trim(), search.MinPrice, search.MaxPrice, search.IdCategory);
        var data = _mapper.Map<IEnumerable<ProductListDto>>(products);
        var result = Result<IEnumerable<ProductListDto>>.Success(data);
        return result;
    }

}
