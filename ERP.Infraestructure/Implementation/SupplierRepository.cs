using ERP.Domain.Entities;
using ERP.Infraestructure.Data;
using ERP.Infraestructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infraestructure.Implementation;

public class SupplierRepository : ISupplierRepository
{
    private readonly AppDbContext _context;

    public SupplierRepository(AppDbContext context)
    {
        _context = context;
    }

    // =====================================================================
    // Lectura (AsNoTracking: solo se leen, no se modifican)
    // =====================================================================

    public async Task<IEnumerable<Supplier>> GetAllAsync()
    {
        var result = await _context.Supplier
            .AsNoTracking()
            .OrderBy(s => s.IdSupplier)
            .ToListAsync();

        return result;
    }

    public async Task<IEnumerable<Supplier>> GetPaginationAsync(int pageNumber, int pageSize)
    {
        var result = await _context.Supplier
            .AsNoTracking()
            .OrderBy(s => s.IdSupplier) // Skip/Take siempre sobre un orden definido
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return result;
    }

    // Total de registros: lo necesita la paginación para calcular TotalPages
    public async Task<int> CountAsync()
    {
        var result = await _context.Supplier.CountAsync();

        return result;
    }

    public async Task<Supplier?> GetByIdAsync(int id)
    {
        var result = await _context.Supplier
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.IdSupplier == id);

        return result;
    }

    // =====================================================================
    // Verificaciones (AnyAsync → SELECT CASE WHEN EXISTS(...): no trae filas)
    // =====================================================================

    public async Task<bool> ExistsAsync(int id)
    {
        var result = await _context.Supplier
            .AnyAsync(s => s.IdSupplier == id);

        return result;
    }

    // ¿Algún producto usa este proveedor? (relación N:N Product–Supplier)
    public async Task<bool> HasProductsAsync(int id)
    {
        var result = await _context.Product
            .AnyAsync(p => p.ProductSuppliers.Any(ps => ps.IdSupplier == id));

        return result;
    }

    // =====================================================================
    // Escritura (siempre async y con un solo SaveChangesAsync por operación)
    // =====================================================================

    public async Task AddAsync(Supplier supplier)
    {
        // IdSupplier es ValueGeneratedNever (no es IDENTITY): el siguiente número se calcula con LINQ,
        // igual que IdInvoice en DevelopmentSeeder
        var lastId = await _context.Supplier.MaxAsync(s => (int?)s.IdSupplier) ?? 0;
        supplier.IdSupplier = lastId + 1;

        _context.Supplier.Add(supplier);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Supplier supplier)
    {
        // Update marca todas las columnas como modificadas (PUT = reemplazo completo)
        _context.Supplier.Update(supplier);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        // ExecuteDeleteAsync envía un DELETE directo a SQL Server: no carga la entidad
        // y no necesita SaveChangesAsync
        await _context.Supplier
            .Where(s => s.IdSupplier == id)
            .ExecuteDeleteAsync();
    }
}
