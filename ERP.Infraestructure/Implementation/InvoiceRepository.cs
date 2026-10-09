using ERP.Domain.Entities;
using ERP.Infraestructure.Data;
using ERP.Infraestructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infraestructure.Implementation;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly AppDbContext _context;

    public InvoiceRepository(AppDbContext context)
    {
        _context = context;
    }

    // Detalle: carga anticipada (Eager Loading) de todas las ramas
    public async Task<Invoice?> GetByIdAsync(int id)
    {
        var result = await _context.Invoice
            .AsNoTracking()
            .Include(i => i.Customer)                 // rama 1: Invoice → Customer
            .Include(i => i.InvoiceItems)             // rama 2: Invoice → InvoiceItem
                .ThenInclude(ii => ii.Product)        //         InvoiceItem → Product
            .Include(i => i.InvoiceItems)             // rama 3: Invoice → InvoiceItem
                .ThenInclude(ii => ii.Tax)            //         InvoiceItem → Tax
            .FirstOrDefaultAsync(i => i.IdInvoice == id);

        return result;
    }

    // Igual que el detalle, filtrado por cliente
    public async Task<IEnumerable<Invoice>> GetByCustomerAsync(string idCustomer)
    {
        var result = await _context.Invoice
            .AsNoTracking()
            .Include(i => i.Customer)
            .Include(i => i.InvoiceItems)
                .ThenInclude(ii => ii.Product)
            .Include(i => i.InvoiceItems)
                .ThenInclude(ii => ii.Tax)
            .Where(i => i.IdCustomer == idCustomer)
            .OrderBy(i => i.IdInvoice)
            .ToListAsync();

        return result;
    }

    // Solo las líneas: se consulta InvoiceItem directamente
    public async Task<IEnumerable<InvoiceItem>> GetLinesAsync(int idInvoice)
    {
        var result = await _context.InvoiceItem
            .AsNoTracking()
            .Include(ii => ii.Product)                // JOIN con Product (ProductName)
            .Include(ii => ii.Tax)                    // JOIN con Tax (Percentage)
            .Where(ii => ii.IdInvoice == idInvoice)
            .OrderBy(ii => ii.LineNumber)
            .ToListAsync();

        return result;
    }
}
