using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
namespace ERP.Infraestructure.Data.Seed
{
    /// <summary>
    /// SEED DE DESARROLLO (UseAsyncSeeding, solo si el ambiente es Development).
    /// Datos ficticios: vendedores, clientes, proveedores, productos y facturas.
    /// Nunca llega a producción.
    /// </summary>
    public static class DevelopmentSeeder
    {
        public static void Seed(DbContext context) =>
        SeedAsync(context, CancellationToken.None).GetAwaiter().GetResult();
        public static async Task SeedAsync(DbContext context, CancellationToken ct)
        {
            // Idempotencia: si ya hay clientes, el seed de desarrollo ya se ejecutó
            if (await context.Set<Customer>().AnyAsync(ct))
                return;
            // Todo o nada: si Migrate dejó una transacción abierta se participa en ella;
            // si no, se abre una propia. Así nunca quedan datos a medias.
            await using var transaction = context.Database.CurrentTransaction is null
            ? await context.Database.BeginTransactionAsync(ct)
            : null;
            // 1. Entidades secundarias (sus FK apuntan a catálogos con IDs estables)
            context.Set<User>().AddRange(DemoData.Sellers());
            context.Set<Customer>().AddRange(DemoData.Customers());
            context.Set<Supplier>().AddRange(DemoData.Suppliers());
            context.Set<Supplier>().AddRange(DemoData.MoreSuppliers()); // NUEVO: 11 al 100
            context.Set<Product>().AddRange(DemoData.Products());
            await context.SaveChangesAsync(ct);
            // 2. Relación Product–Supplier generada a partir de SupplierByCategory
            var supplierIds = await context.Set<Supplier>()
            .ToDictionaryAsync(s => s.SupplierName, s => s.IdSupplier, ct);
            var products = await context.Set<Product>()
            .Select(p => new { p.IdProduct, p.IdCategory })
            .ToListAsync(ct);
            var productSuppliers = products
            .Where(p => DemoData.SupplierByCategory.ContainsKey(p.IdCategory))
            .Select(p => new ProductSupplier
            {
                IdProduct = p.IdProduct,
                IdSupplier = supplierIds[DemoData.SupplierByCategory[p.IdCategory]]
            });
            context.Set<ProductSupplier>().AddRange(productSuppliers);
            await context.SaveChangesAsync(ct);
            // 3. Entidades operativas: facturas y detalle
            var sellerIds = await context.Set<User>()
            .Where(u => u.IdRole == SeedIds.RoleVendedor)
            .ToDictionaryAsync(u => u.Login, u => u.IdUser, ct);
            var productNames = DemoData.Invoices
            .SelectMany(i => i.Items)
            .Select(i => i.ProductName)
            .Distinct()
            .ToList();
            var catalog = await context.Set<Product>()
            .Where(p => productNames.Contains(p.ProductName))
            .ToDictionaryAsync(p => p.ProductName, p => new { p.IdProduct, p.Price }, ct);
            // IdInvoice es ValueGeneratedNever: se calcula el siguiente número
            var nextInvoiceId = (await context.Set<Invoice>().MaxAsync(i => (int?)i.IdInvoice, ct) ?? 0) + 1;
            foreach (var demo in DemoData.Invoices)
            {
                var idInvoice = nextInvoiceId++;
                context.Set<Invoice>().Add(new Invoice
                {
                    IdInvoice = idInvoice,
                    IdCustomer = demo.CustomerId,
                    IdUser = sellerIds[demo.SellerLogin],
                    IdInvoiceStatus = SeedIds.InvoiceStatusPagada,
                    IdPaymentType = SeedIds.PaymentTypeTarjetaCredito,
                    IdCreditCard = SeedIds.CreditCardMastercard,
                    CreditCardNumber = demo.CardLast4, // solo los últimos 4 dígitos
                    InvoiceDate = demo.InvoiceDate
                });
                context.Set<InvoiceItem>().AddRange(demo.Items.Select((item, index) => new InvoiceItem
                {
                    IdInvoice = idInvoice,
                    LineNumber = index + 1,
                    IdProduct = catalog[item.ProductName].IdProduct,
                    IdTax = SeedIds.TaxImpuestoVentas,
                    Quantity = item.Quantity,
                    Price = catalog[item.ProductName].Price // precio congelado al momento de la venta
                }));
            }
            await context.SaveChangesAsync(ct);
            if (transaction is not null)
                await transaction.CommitAsync(ct);
        }
    }
}