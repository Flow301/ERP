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

    // =====================================================================
    // Lectura (AsNoTracking: solo se leen, no se modifican)
    // =====================================================================

    public async Task<IEnumerable<Warehouse>> GetAllAsync()
    {
        var result = await _context.Warehouse
            .AsNoTracking()
            .OrderBy(w => w.IdWarehouse)
            .ToListAsync();

        return result;
    }

    public async Task<IEnumerable<Warehouse>> GetPaginationAsync(int pageNumber, int pageSize)
    {
        var result = await _context.Warehouse
            .AsNoTracking()
            .OrderBy(w => w.IdWarehouse) // Skip/Take siempre sobre un orden definido
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return result;
    }

    // Total de registros: lo necesita la paginación para calcular TotalPages
    public async Task<int> CountAsync()
    {
        var result = await _context.Warehouse.CountAsync();

        return result;
    }

    public async Task<Warehouse?> GetByIdAsync(int id)
    {
        var result = await _context.Warehouse
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.IdWarehouse == id);

        return result;
    }

    // =====================================================================
    // Verificaciones (AnyAsync → SELECT CASE WHEN EXISTS(...): no trae filas)
    // =====================================================================

    public async Task<bool> ExistsAsync(int id)
    {
        var result = await _context.Warehouse
            .AnyAsync(w => w.IdWarehouse == id);

        return result;
    }

    // ¿Otra bodega ya usa este nombre? En el PUT se excluye la propia bodega,
    // así puede guardarse sin cambiar el nombre. (La intercalación de SQL Server no distingue mayúsculas.)
    public async Task<bool> ExistsByNameAsync(string name, int? excludeId = null)
    {
        var result = await _context.Warehouse
            .AnyAsync(w => w.WarehouseName == name && w.IdWarehouse != excludeId);

        return result;
    }

    // ¿Algún producto está en esta bodega? (relación 1:N por Product.IdWarehouse)
    public async Task<bool> HasProductsAsync(int id)
    {
        var result = await _context.Product
            .AnyAsync(p => p.IdWarehouse == id);

        return result;
    }

    // =====================================================================
    // Escritura (siempre async y con un solo SaveChangesAsync por operación)
    // =====================================================================

    public async Task AddAsync(Warehouse warehouse)
    {
        // IdWarehouse es ValueGeneratedNever (no es IDENTITY): el siguiente número se calcula con LINQ
        var lastId = await _context.Warehouse.MaxAsync(w => (int?)w.IdWarehouse) ?? 0;
        warehouse.IdWarehouse = lastId + 1;

        _context.Warehouse.Add(warehouse);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Warehouse warehouse)
    {
        // Update marca todas las columnas como modificadas (PUT = reemplazo completo)
        _context.Warehouse.Update(warehouse);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        // ExecuteDeleteAsync envía un DELETE directo a SQL Server: no carga la entidad
        // y no necesita SaveChangesAsync
        await _context.Warehouse
            .Where(w => w.IdWarehouse == id)
            .ExecuteDeleteAsync();
    }
}
