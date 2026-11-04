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

    // ===================================================================== 
    // CONSULTAS (laboratorio anterior, sin cambios) 
    // ===================================================================== 

    // Endpoint 1: una factura con cliente, líneas, productos e impuesto
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

    // Endpoint 2: todas las facturas de un cliente (mismas relaciones que el 1) 
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
    // Endpoint 3: solo las líneas de una factura 
    public async Task<IEnumerable<InvoiceItem>> GetLinesAsync(int idInvoice)
    {
        var result = await _context.InvoiceItem
            .AsNoTracking()
            .Include(ii => ii.Product)
            .Include(ii => ii.Tax)
            .Where(ii => ii.IdInvoice == idInvoice)
            .OrderBy(ii => ii.LineNumber)
            .ToListAsync();

        return result;
    }

    // ===================================================================== 
    // NUEVO  VALIDACIONES DE INTEGRIDAD REFERENCIAL 
    // Set<T>() funciona aunque el DbSet de esa tabla tenga otro nombre en AppDbContext. 
    // ===================================================================== 

    public async Task<bool> CustomerExistsAsync(string idCustomer)
    {
        var result = await _context.Customer
            .AnyAsync(c => c.IdCustomer == idCustomer);

        return result;
    }

    public async Task<bool> ActiveUserExistsAsync(int idUser)
    {
        var result = await _context.Set<User>()
          .AnyAsync(u => u.IdUser == idUser && u.Status);

        return result;
    }

    public async Task<bool> PaymentTypeExistsAsync(int idPaymentType)
    {
        var result = await _context.Set<PaymentType>()
            .AnyAsync(pt => pt.IdPaymentType == idPaymentType);

        return result;
    }

    public async Task<bool> InvoiceStatusExistsAsync(int idInvoiceStatus)
    {
        var result = await _context.Set<InvoiceStatus>()
            .AnyAsync(s => s.IdInvoiceStatus == idInvoiceStatus);

        return result;
    }

    public async Task<bool> CreditCardExistsAsync(int idCreditCard)
    {
        var result = await _context.Set<CreditCard>()
            .AnyAsync(cc => cc.IdCreditCard == idCreditCard);

        return result;
    }
    // Una sola consulta para todos los productos de la factura (WHERE IdProduct IN (...)) 
    public async Task<IEnumerable<Product>> GetProductsAsync(IEnumerable<int> ids)
    {
        var result = await _context.Product
            .AsNoTracking()
            .Where(p => ids.Contains(p.IdProduct))
            .ToListAsync();

        return result;
    }

    public async Task<IEnumerable<Tax>> GetTaxesAsync(IEnumerable<int> ids)
    {
        var result = await _context.Set<Tax>()
            .AsNoTracking()
            .Where(t => ids.Contains(t.IdTax))
            .ToListAsync();

        return result;
    }

    // ===================================================================== 
    // NUEVO  PROCESO TRANSACCIONAL: todo o nada 
    // ===================================================================== 

    public async Task<int?> CreateAsync(Invoice invoice)
    {
        // La estrategia de ejecución permite usar una transacción propia aunque el 
        // DbContext tenga EnableRetryOnFailure. Si hay un fallo transitorio, repite TODO el bloque.
        var strategy = _context.Database.CreateExecutionStrategy();
        var result = await strategy.ExecuteAsync(async () =>
        {
        _context.ChangeTracker.Clear();      // si el bloque se repite, empieza limpio 

        // BEGIN TRANSACTION. Si algo lanza una excepción antes del Commit, 
        // "await using" revierte la transacción al salir del bloque. 
        await using var transaction = await _context.Database.BeginTransactionAsync();

        // 1. Consecutivo: IdInvoice es ValueGeneratedNever (igual que en DevelopmentSeeder)
            var lastId = await _context.Invoice.MaxAsync(i => (int?)i.IdInvoice) ?? 0;
        invoice.IdInvoice = lastId + 1;

        foreach (var item in invoice.InvoiceItems)
            item.IdInvoice = invoice.IdInvoice;

        // 2. Encabezado + detalle: un solo SaveChangesAsync inserta Invoice y sus InvoiceItem 
        _context.Invoice.Add(invoice);
        await _context.SaveChangesAsync();

        // 3. Rebajar inventario. ExecuteUpdateAsync va directo a SQL Server (no usa SaveChanges);
        //    la condición StockQuantity >= cantidad es la última barrera contra vender sin existencias.
            foreach (var item in invoice.InvoiceItems)
        {
            var idProduct = item.IdProduct;
            var quantity = item.Quantity;

            var updated = await _context.Product
 .Where(p => p.IdProduct == idProduct && p.Status && p.StockQuantity >=
quantity)
                    .ExecuteUpdateAsync(s => s.SetProperty(p => p.StockQuantity, p =>
p.StockQuantity - quantity));

                if (updated == 0)
                {
                    // ROLLBACK: se deshacen la factura, sus líneas y las rebajas ya hechas 
                    await transaction.RollbackAsync();
                    _context.ChangeTracker.Clear();
                    return (int?)null;
                }
            }

            // COMMIT: a partir de aquí los cambios son permanentes y visibles para todos 
            await transaction.CommitAsync();
            return (int?)invoice.IdInvoice;
        });

        return result;
    }

    // ===================================================================== 
    // RETO  PROCESO TRANSACCIONAL: anular una factura (todo o nada) 
    // ===================================================================== 

    public async Task<bool> VoidAsync(int idInvoice, int idStatusVoided)
    {
        // Misma estrategia de ejecución que CreateAsync: si hay un fallo transitorio, repite TODO el bloque.
        var strategy = _context.Database.CreateExecutionStrategy();
        var result = await strategy.ExecuteAsync(async () =>
        {
            _context.ChangeTracker.Clear();      // si el bloque se repite, empieza limpio 

            // BEGIN TRANSACTION. Si algo lanza una excepción antes del Commit, 
            // "await using" revierte la transacción al salir del bloque. 
            await using var transaction = await _context.Database.BeginTransactionAsync();

            // 1. Cambiar el estado. La condición "todavía no está anulada" va DENTRO del UPDATE:
            //    si dos usuarios anulan la misma factura a la vez, solo uno actualiza la fila
            //    y el inventario no se devuelve dos veces.
            var updated = await _context.Invoice
                .Where(i => i.IdInvoice == idInvoice && i.IdInvoiceStatus != idStatusVoided)
                .ExecuteUpdateAsync(s => s.SetProperty(i => i.IdInvoiceStatus, idStatusVoided));

            if (updated == 0)
            {
                // ROLLBACK: no se cambió nada 
                await transaction.RollbackAsync();
                return false;
            }

            // 2. Devolver al inventario la cantidad de cada línea (StockQuantity + Quantity).
            //    ExecuteUpdateAsync va directo a SQL Server, dentro de la misma transacción.
            var lines = await _context.InvoiceItem
                .AsNoTracking()
                .Where(ii => ii.IdInvoice == idInvoice)
                .Select(ii => new { ii.IdProduct, ii.Quantity })
                .ToListAsync();

            foreach (var line in lines)
            {
                var idProduct = line.IdProduct;
                var quantity = line.Quantity;

                await _context.Product
                    .Where(p => p.IdProduct == idProduct)
                    .ExecuteUpdateAsync(s => s.SetProperty(p => p.StockQuantity, p => p.StockQuantity + quantity));
            }

            // COMMIT: el estado y el inventario cambian juntos 
            await transaction.CommitAsync();
            return true;
        });

        return result;
    }
}
