using ERP.Domain.DTO;
using ERP.Domain.Entities;
using ERP.Infraestructure.Data;
using ERP.Infraestructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infraestructure.Implementation;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    // =====================================================================
    // LECTURA (laboratorios de LINQ)
    // AsNoTracking: solo se leen. Where(p => p.Status): un producto con
    // eliminación lógica deja de aparecer en todas las consultas.
    // =====================================================================

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        var result = await _context.Product
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Warehouse)
            .Where(p => p.Status)
            .OrderBy(p => p.IdProduct)
            .ToListAsync();

        return result;
    }

    public async Task<IEnumerable<Product>> GetAllOrderDescAsync()
    {
        var result = await _context.Product
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Warehouse)
            .Where(p => p.Status)
            .OrderByDescending(p => p.ProductName)
            .ToListAsync();

        return result;
    }

    public async Task<IEnumerable<Product>> FindByNameAsync(string name)
    {
        var result = await _context.Product
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Warehouse)
            .Where(p => p.Status && p.ProductName.Contains(name))
            .OrderBy(p => p.ProductName)
            .ToListAsync();

        return result;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        var result = await _context.Product
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Warehouse)
            .Include(p => p.ProductSuppliers)
                .ThenInclude(ps => ps.Supplier)
            .FirstOrDefaultAsync(p => p.IdProduct == id && p.Status);

        return result;
    }

    public async Task<IEnumerable<Product>> GetPaginationAsync(int pageNumber, int pageSize)
    {
        var result = await _context.Product
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Warehouse)
            .Where(p => p.Status)
            .OrderBy(p => p.IdProduct)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return result;
    }

    public async Task<IEnumerable<Product>> GetPriceAsync(decimal price)
    {
        var result = await _context.Product
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Warehouse)
            .Where(p => p.Price >= price && p.Status)
            .OrderBy(p => p.Price)
            .ToListAsync();

        return result;
    }

    public async Task<IEnumerable<Product>> GetTop10PriceAsync()
    {
        var result = await _context.Product
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Warehouse)
            .Where(p => p.Status)
            .OrderByDescending(p => p.Price)
            .ThenBy(p => p.ProductName)
            .Take(10)
            .ToListAsync();

        return result;
    }

    public async Task<int> CountAsync()
    {
        var result = await _context.Product.CountAsync(p => p.Status);

        return result;
    }

    public async Task<bool> ExistsByDescriptionAsync(string description)
    {
        var result = await _context.Product
            .AnyAsync(p => p.Status && p.Description.Contains(description));

        return result;
    }

    public async Task<IEnumerable<Product>> GetByCategoryAsync(int idCategory)
    {
        var result = await _context.Product
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Warehouse)
            .Where(p => p.Status && p.IdCategory == idCategory)
            .OrderBy(p => p.ProductName)
            .ToListAsync();

        return result;
    }

    public async Task<IEnumerable<Product>> GetByCategoryNameAsync(string name)
    {
        var result = await _context.Product
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Warehouse)
            .Where(p => p.Status && p.Category.CategoryName.Contains(name))
            .OrderBy(p => p.ProductName)
            .ToListAsync();

        return result;
    }

    public async Task<IEnumerable<Product>> GetByWarehouseIdAsync(int idWarehouse)
    {
        var result = await _context.Product
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Warehouse)
            .Where(p => p.Status && p.IdWarehouse == idWarehouse)
            .OrderBy(p => p.ProductName)
            .ToListAsync();

        return result;
    }

    public async Task<IEnumerable<Product>> GetByWarehouseNameAsync(string name)
    {
        var result = await _context.Product
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Warehouse)
            .Where(p => p.Status && p.Warehouse.WarehouseName.Contains(name))
            .OrderBy(p => p.ProductName)
            .ToListAsync();

        return result;
    }

    public async Task<IEnumerable<CategoryStatsDto>> GetStatsByCategoryAsync()
    {
        var result = await _context.Product
            .Where(p => p.Status)
            .GroupBy(p => new { p.IdCategory, p.Category.CategoryName })
            .Select(g => new CategoryStatsDto
            {
                IdCategory = g.Key.IdCategory,
                CategoryName = g.Key.CategoryName,
                ProductCount = g.Count(),
                MinPrice = g.Min(p => p.Price),
                MaxPrice = g.Max(p => p.Price),
                AveragePrice = Math.Round(g.Average(p => p.Price), 2),
                InventoryValue = g.Sum(p => p.Price * p.StockQuantity)
            })
            .OrderBy(s => s.CategoryName)
            .ToListAsync();

        return result;
    }

    // =====================================================================
    // NUEVO — Verificaciones de las relaciones (AnyAsync: no trae filas)
    // =====================================================================

    public async Task<bool> CategoryExistsAsync(int idCategory)
    {
        var result = await _context.Category
            .AnyAsync(c => c.IdCategory == idCategory);

        return result;
    }

    public async Task<bool> WarehouseExistsAsync(int idWarehouse)
    {
        var result = await _context.Warehouse
            .AnyAsync(w => w.IdWarehouse == idWarehouse);

        return result;
    }

    // Devuelve cuáles de los Ids recibidos existen y están activos
    // SQL: SELECT IdSupplier FROM Supplier WHERE IdSupplier IN (...) AND Status = 1
    public async Task<IEnumerable<int>> GetActiveSupplierIdsAsync(IEnumerable<int> supplierIds)
    {
        var result = await _context.Supplier
            .Where(s => supplierIds.Contains(s.IdSupplier) && s.Status)
            .Select(s => s.IdSupplier)
            .ToListAsync();

        return result;
    }

    // =====================================================================
    // NUEVO — Escritura
    // Sin AsNoTracking: el ChangeTracker debe rastrear la entidad para saber
    // qué cambió cuando se llame SaveChangesAsync().
    // =====================================================================

    // Editar: se incluye la colección que se va a sincronizar (y solo esa)
    public async Task<Product?> GetForUpdateAsync(int id)
    {
        var result = await _context.Product
            .Include(p => p.ProductSuppliers)
            .FirstOrDefaultAsync(p => p.IdProduct == id && p.Status);

        return result;
    }

    // Eliminar: solo cambia Status, no hace falta ninguna relación
    public async Task<Product?> GetForDeleteAsync(int id)
    {
        var result = await _context.Product
            .FirstOrDefaultAsync(p => p.IdProduct == id && p.Status);

        return result;
    }

    // Crear: Add marca el producto Y sus ProductSuppliers como Added;
    // SaveChangesAsync inserta todo el grafo en una sola transacción
    public async Task AddAsync(Product product)
    {
        // IdProduct es ValueGeneratedNever (no es IDENTITY): el siguiente número se calcula con LINQ
        var lastId = await _context.Product.MaxAsync(p => (int?)p.IdProduct) ?? 0;
        product.IdProduct = lastId + 1;

        _context.Product.Add(product);
        await _context.SaveChangesAsync();
    }

    // Marca las filas de la tabla puente como Deleted (el DELETE se envía en SaveChangesAsync)
    public void RemoveSupplierLinks(IEnumerable<ProductSupplier> links)
    {
        _context.RemoveRange(links);
    }

    // Un solo SaveChangesAsync por operación: todo o nada
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    // Filtros + búsqueda + ordenamiento + paginación, en UNA consulta a SQL Server
    public async Task<(IEnumerable<Product> Items, int TotalRecords)>
SearchAsync(ProductSearchDto search)
    {
        // 1. Punto de partida: IQueryable todavía no ejecuta nada en la base
        IQueryable<Product> query = _context.Product
        .AsNoTracking()
        .Include(p => p.Category)
        .Include(p => p.Warehouse)
        .Where(p => p.Status == search.Status); // estado: activos por defecto
                                                // 2. Filtros y búsqueda: cada uno se agrega SOLO si viene en la petición
        if (!string.IsNullOrWhiteSpace(search.Name))
        {
            var name = search.Name.Trim();
            query = query.Where(p => p.ProductName.Contains(name));
        }
        if (search.MinPrice.HasValue)
            query = query.Where(p => p.Price >= search.MinPrice.Value);
        if (search.MaxPrice.HasValue)
            query = query.Where(p => p.Price <= search.MaxPrice.Value);
        if (search.IdCategory.HasValue)
            query = query.Where(p => p.IdCategory == search.IdCategory.Value);
        if (search.IdSupplier.HasValue) // relación N:N → Any()
            query = query.Where(p => p.ProductSuppliers.Any(ps => ps.IdSupplier ==
           search.IdSupplier.Value));
        // 3. Total ANTES de paginar: SELECT COUNT(*) con los mismos filtros
        var totalRecords = await query.CountAsync();
        // 4. Ordenamiento: lista blanca con switch (nunca el texto del cliente directo a la consulta)
 var descending = string.Equals(search.SortDirection, "desc",
StringComparison.OrdinalIgnoreCase);
        IOrderedQueryable<Product> ordered = search.SortBy?.ToLowerInvariant() switch
        {
            "price" => descending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p =>
           p.Price),
            "stock" => descending ? query.OrderByDescending(p => p.StockQuantity) :
           query.OrderBy(p => p.StockQuantity),
            "date" => descending ? query.OrderByDescending(p => p.InsertDate) :
           query.OrderBy(p => p.InsertDate),
            _ => descending ? query.OrderByDescending(p => p.ProductName) :
           query.OrderBy(p => p.ProductName)
        };
        // 5. Desempate por Id: con valores repetidos (mismo precio) las páginas no se mezclan
        // 6. Paginación: OFFSET/FETCH en SQL Server
        var items = await ordered
        .ThenBy(p => p.IdProduct)
        .Skip((search.PageNumber - 1) * search.PageSize)
        .Take(search.PageSize)
        .ToListAsync();
        var result = (items, totalRecords);
        return result;
    }


    // Regla de negocio: no dos productos ACTIVOS con el mismo nombre.
    // SQL Server compara sin distinguir mayúsculas (collation por defecto): no hace falta ToLower()
    public async Task<bool> ExistsByNameAsync(string productName, int? excludeId = null)
    {
        var result = await _context.Product
        .AnyAsync(p => p.Status
        && p.ProductName == productName
        && (excludeId == null || p.IdProduct != excludeId));
        return result;
    }
}
