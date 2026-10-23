using ERP.Application.Interfaces;
using ERP.Domain.DTO;
using ERP.Domain.DTO.PatternResult;
using ERP.Domain.Entities;
using ERP.Infraestructure.Interfaces;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ERP.Application.Implementation;



public class ProductService : IProductService

{
    // Regla de paginación: igual que en SupplierService
    public const int MaxPageSize = 50;
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductService> _logger; // NUEVO 6-1
    public ProductService(IProductRepository repository, IMapper mapper,
    ILogger<ProductService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    // =====================================================================
    // Laboratorios de LINQ — 6-1: ahora devuelven Result<T>
    // El repositorio no cambia: sigue devolviendo entidades
    // =====================================================================
    public async Task<Result<IEnumerable<ProductListDto>>> GetAllAsync()
    {
        var products = await _repository.GetAllAsync();
        var data = _mapper.Map<IEnumerable<ProductListDto>>(products);
        var result = ListResult(data);
        return result;
    }
    public async Task<Result<IEnumerable<ProductListDto>>> GetAllOrderDescAsync()
    {
        var products = await _repository.GetAllOrderDescAsync();
        var data = _mapper.Map<IEnumerable<ProductListDto>>(products);
        var result = ListResult(data);
        return result;
    }
    public async Task<Result<IEnumerable<ProductListDto>>> FindByNameAsync(string name)
    {
        var products = await _repository.FindByNameAsync(name.Trim());
        var data = _mapper.Map<IEnumerable<ProductListDto>>(products);
        var result = ListResult(data);
        return result;
    }
    // Detalle: el repositorio devuelve la ENTIDAD con sus relaciones;
    // el mapper arma los DTOs anidados y los campos calculados.
    public async Task<Result<ProductDetailDto>> GetByIdAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product is null)
            return Result<ProductDetailDto>.Failure(404, "Not Found", $"Product {id} doesn't exist!");
           
            var data = _mapper.Map<ProductDetailDto>(product);
        var result = Result<ProductDetailDto>.Success(data);
        return result;
    }
    // Misma regla de paginación que SupplierService: 400 si los parámetros no son válidos
    public async Task<Result<IEnumerable<ProductListDto>>> GetPaginationAsync(int
pageNumber, int pageSize)
    {
        if (pageNumber < 1 || pageSize < 1 || pageSize > MaxPageSize)
            return Result<IEnumerable<ProductListDto>>.Failure(400, "Bad Request",
            $"pageNumber must be 1 or more and pageSize between 1 and {MaxPageSize}!");
        var products = await _repository.GetPaginationAsync(pageNumber, pageSize);
        var totalRecords = await _repository.CountAsync();
        var data = _mapper.Map<IEnumerable<ProductListDto>>(products);
        var result = data.Any()
        ? Result<IEnumerable<ProductListDto>>.Paged(data, pageNumber, pageSize,
       totalRecords)
        : Result<IEnumerable<ProductListDto>>.Failure(404, "Not Found", "No product found!");
 return result;
    }
    public async Task<Result<IEnumerable<ProductListDto>>> GetPriceAsync(decimal price)
    {
        var products = await _repository.GetPriceAsync(price);
        var data = _mapper.Map<IEnumerable<ProductListDto>>(products);
        var result = ListResult(data);
        return result;
    }
    public async Task<Result<IEnumerable<ProductListDto>>> GetTop10PriceAsync()
    {
        var products = await _repository.GetTop10PriceAsync();
        var data = _mapper.Map<IEnumerable<ProductListDto>>(products);
        var result = ListResult(data);
        return result;
    }
    // Un número o un bool no necesitan mapper
    public async Task<Result<int>> CountAsync()
    {
        var count = await _repository.CountAsync();
        var result = Result<int>.Success(count);
        return result;
    }
    public async Task<Result<bool>> ExistsByDescriptionAsync(string description)
    {
        var exists = await _repository.ExistsByDescriptionAsync(description.Trim());
        var result = Result<bool>.Success(exists);
        return result;
    }
    // Categoría sin productos → 200 con [] (no es un error): no usa ListResult
    public async Task<Result<IEnumerable<ProductListDto>>> GetByCategoryAsync(int
   idCategory)
    {
        var products = await _repository.GetByCategoryAsync(idCategory);
        var data = _mapper.Map<IEnumerable<ProductListDto>>(products);
        var result = Result<IEnumerable<ProductListDto>>.Success(data);
        return result;
    }
    public async Task<Result<IEnumerable<ProductListDto>>>
   GetByCategoryNameAsync(string name)
    {
        var products = await _repository.GetByCategoryNameAsync(name.Trim());
        var data = _mapper.Map<IEnumerable<ProductListDto>>(products);
        var result = ListResult(data);
        return result;
    }
    public async Task<Result<IEnumerable<ProductListDto>>> GetByWarehouseIdAsync(int
   idWarehouse)
    {
        var products = await _repository.GetByWarehouseIdAsync(idWarehouse);
        var data = _mapper.Map<IEnumerable<ProductListDto>>(products);
        var result = ListResult(data);
        return result;
    }
    public async Task<Result<IEnumerable<ProductListDto>>>
   GetByWarehouseNameAsync(string name)
    {
        var products = await _repository.GetByWarehouseNameAsync(name.Trim());
        var data = _mapper.Map<IEnumerable<ProductListDto>>(products);
        var result = ListResult(data);
        return result;
    }
    // Excepción: el GroupBy ya llega como DTO desde el repositorio, no necesita mapper
    public async Task<Result<IEnumerable<CategoryStatsDto>>> GetStatsByCategoryAsync()
    {
        var data = await _repository.GetStatsByCategoryAsync();
        var result = ListResult(data);
        return result;
    }
    // ---------------------------------------------------------------------
    // Lista vacía → 404 (el mismo código que respondían los controllers),
    // ahora con el formato Result<T>.Genérico: sirve para cualquier DTO.
 // ---------------------------------------------------------------------
 private static Result<IEnumerable<T>> ListResult<T>(IEnumerable<T> data)
    {
        var result = data.Any()
        ? Result<IEnumerable<T>>.Success(data)
        : Result<IEnumerable<T>>.Failure(404, "Not Found", "No products found!");
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
        {
            _logger.LogWarning("Intento de crear producto con nombre duplicado: {ProductName}",
           product.ProductName); // NUEVO 6-1
            return Result<ProductDetailDto>.Failure(409, "Conflict",
            $"Ya existe un producto con el nombre '{product.ProductName}'.");
        }


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

        _logger.LogInformation("Producto {IdProduct} creado: {ProductName}", entity.IdProduct,
entity.ProductName); // NUEVO 6-1
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
        {
            _logger.LogWarning("Intento de renombrar el producto {IdProduct} con un nombre duplicado: { ProductName}",
 id, product.ProductName); // NUEVO 6-1
            return Result<ProductDetailDto>.Failure(409, "Conflict",
            $"Ya existe un producto con el nombre '{product.ProductName}'.");
        }

        if (error is not null)
            return Result<ProductDetailDto>.Failure(400, "Bad Request", error);

        // Copia los valores del DTO SOBRE la entidad rastreada:
        // el ChangeTracker compara con su snapshot y marca solo las columnas que cambiaron
        _mapper.Map(product, entity);

        SyncSuppliers(entity, product.SupplierIds);

        await _repository.SaveChangesAsync();     // UPDATE + INSERT/DELETE en ProductSupplier, una transacción

        _logger.LogInformation("Producto {IdProduct} actualizado", id); // NUEVO 6-1

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

        _logger.LogInformation("Producto {IdProduct} desactivado (borrado lógico)", id); // NUEVO 

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
