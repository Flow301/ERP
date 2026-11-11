using ERP.Domain.Entities;
using ERP.Infraestructure.Data;
using ERP.Infraestructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infraestructure.Implementation;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly AppDbContext _context;

    public WarehouseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Warehouse>> GetAllAsync()
    {
        return await _context.Set<Warehouse>()
            .AsNoTracking()
            .OrderBy(w => w.IdWarehouse)
            .ToListAsync();
    }

    public async Task<IEnumerable<Warehouse>> GetPaginationAsync(
        int pageNumber,
        int pageSize)
    {
        return await _context.Set<Warehouse>()
            .AsNoTracking()
            .OrderBy(w => w.IdWarehouse)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> CountAsync()
    {
        return await _context.Set<Warehouse>().CountAsync();
    }

    public async Task<Warehouse?> GetByIdAsync(int id)
    {
        return await _context.Set<Warehouse>()
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.IdWarehouse == id);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Set<Warehouse>()
            .AnyAsync(w => w.IdWarehouse == id);
    }

    // Regla de negocio: nombre de bodega único.
    // excludeId: al editar, la propia bodega no cuenta como duplicado
    public async Task<bool> ExistsByNameAsync(string name, int? excludeId = null)
    {
        var result = await _context.Warehouse
            .AnyAsync(w => w.WarehouseName == name.Trim()
                        && (excludeId == null || w.IdWarehouse != excludeId));

        return result;
    }

    public async Task<bool> HasProductsAsync(int id)
    {
        return await _context.Set<Product>()
            .AnyAsync(p => p.IdWarehouse == id);
    }

    public async Task AddAsync(Warehouse warehouse)
    {
        warehouse.WarehouseName = warehouse.WarehouseName.Trim();

        // IdWarehouse es ValueGeneratedNever (no es IDENTITY): el siguiente número se calcula con LINQ
        var lastId = await _context.Set<Warehouse>().MaxAsync(w => (int?)w.IdWarehouse) ?? 0;
        warehouse.IdWarehouse = lastId + 1;

        _context.Set<Warehouse>().Add(warehouse);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Warehouse warehouse)
    {
        warehouse.WarehouseName = warehouse.WarehouseName.Trim();

        _context.Set<Warehouse>().Update(warehouse);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var warehouse = await _context.Set<Warehouse>()
            .FirstAsync(w => w.IdWarehouse == id);

        _context.Set<Warehouse>().Remove(warehouse);
        await _context.SaveChangesAsync();
    }
}
