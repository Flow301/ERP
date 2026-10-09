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
    // Laboratorio anterior — misma consulta + Include de Category y Warehouse,
    // porque ProductListDto muestra CategoryName y WarehouseName
    // =====================================================================

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        var result = await _context.Product
            .AsNoTracking()                       // solo lectura: EF no rastrea las entidades
            .Include(p => p.Category)             // JOIN con Category
            .Include(p => p.Warehouse)            // JOIN con Warehouse
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
            .Where(p => p.ProductName.Contains(name))
            .OrderBy(p => p.ProductName)
            .ToListAsync();

        return result;
    }

    // Detalle: carga anticipada (Eager Loading) de todas las ramas
    public async Task<Product?> GetByIdAsync(int id)
    {
        var result = await _context.Product
            .AsNoTracking()
            .Include(p => p.Category)                 // rama 1: Product → Category
            .Include(p => p.Warehouse)                // rama 2: Product → Warehouse
            .Include(p => p.ProductSuppliers)         // rama 3: Product → ProductSupplier
                .ThenInclude(ps => ps.Supplier)       //         ProductSupplier → Supplier
            .FirstOrDefaultAsync(p => p.IdProduct == id);

        return result;
    }

    public async Task<IEnumerable<Product>> GetPaginationAsync(int pageNumber, int pageSize)
    {
        var result = await _context.Product
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Warehouse)
            .OrderBy(p => p.IdProduct)            // Skip/Take siempre sobre un orden definido
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
            .ThenBy(p => p.ProductName)           // desempate: tres productos cuestan 119.99
            .Take(10)
            .ToListAsync();

        return result;
    }

    public async Task<int> CountAsync()
    {
        var result = await _context.Product.CountAsync();

        return result;
    }

    // Reto A (sin cambios)
    public async Task<bool> ExistsByDescriptionAsync(string description)
    {
        var result = await _context.Product
            .AnyAsync(p => p.Description.Contains(description));

        return result;
    }

    // Reto B
    public async Task<IEnumerable<Product>> GetByCategoryAsync(int idCategory)
    {
        var result = await _context.Product
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Warehouse)
            .Where(p => p.IdCategory == idCategory)
            .OrderBy(p => p.ProductName)
            .ToListAsync();

        return result;
    }

    // =====================================================================
    // NUEVO — LINQ avanzado
    // =====================================================================

    // Filtro sobre una relación: el Where genera el JOIN por sí solo;
    // el Include se agrega porque el mapper necesita CategoryName y WarehouseName
    public async Task<IEnumerable<Product>> GetByCategoryNameAsync(string name)
    {
        var result = await _context.Product
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Warehouse)
            .Where(p => p.Category.CategoryName.Contains(name))
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
            .Where(p => p.IdWarehouse == idWarehouse)
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
            .Where(p => p.Warehouse.WarehouseName.Contains(name))
            .OrderBy(p => p.ProductName)
            .ToListAsync();

        return result;
    }

    // EXCEPCIÓN — Agregación: el resultado de un GroupBy no es una entidad.
    // El cálculo debe hacerse en SQL (si no, habría que traer todos los productos a memoria),
    // así que el repositorio devuelve directamente CategoryStatsDto.
    // SELECT p.IdCategory, c.CategoryName, COUNT(*), MIN(p.Price), MAX(p.Price),
    //        ROUND(AVG(p.Price), 2), SUM(p.Price * p.StockQuantity)
    // FROM Product p JOIN Category c ... GROUP BY p.IdCategory, c.CategoryName
    public async Task<IEnumerable<CategoryStatsDto>> GetStatsByCategoryAsync()
    {
        var result = await _context.Product
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
}
