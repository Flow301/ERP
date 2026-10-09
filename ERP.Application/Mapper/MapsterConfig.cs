using ERP.Domain.DTO;
using ERP.Domain.Entities;
using Mapster;

namespace ERP.Application.Mapper;

public static class MapsterConfig
{
    // Regla de negocio: por debajo de este stock el producto se considera "Bajo"
    public const int LowStockLimit = 15;

    //Reto: Montos de una línea, redondeados a 2 decimales (24.99 × 13 % = 3.2487 → 3.25)
    public static decimal LineSubtotal(InvoiceItem item) =>
        Math.Round(item.Price * item.Quantity, 2, MidpointRounding.AwayFromZero);
    //Reto:
    public static decimal LineTax(InvoiceItem item) =>
        Math.Round(item.Price * item.Quantity * item.Tax.Percentage / 100, 2, MidpointRounding.AwayFromZero);

    public static void RegisterMaps()
    {
        // =====================================================================
        // NUEVO: Product → ProductListDto (listas)
        // =====================================================================
        TypeAdapterConfig<Product, ProductListDto>
            .NewConfig()
            // IdProduct, ProductName, Price y StockQuantity se copian solos (mismo nombre)
            // Datos de las relaciones cargadas con Include en el repositorio
            .Map(dest => dest.CategoryName, src => src.Category.CategoryName)
            .Map(dest => dest.WarehouseName, src => src.Warehouse.WarehouseName)
            // Campo calculado
            .Map(dest => dest.InventoryValue, src => src.Price * src.StockQuantity);

        // =====================================================================
        // Product → ProductDetailDto (detalle)
        // =====================================================================
        TypeAdapterConfig<Product, ProductDetailDto>
            .NewConfig()
            // ----- Laboratorio anterior (se conservan) -----
            // Tienen el mismo nombre en la entidad y en el DTO, así que Mapster las copiaría solo;
            // se dejan explícitas como referencia.
            .Map(dest => dest.IdProduct, src => src.IdProduct)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.StockQuantity, src => src.StockQuantity)
            .Map(dest => dest.Image, src => src.Image)
            .Map(dest => dest.ProductName, src => src.ProductName)
            .Map(dest => dest.InsertDate, src => src.InsertDate)
            .Map(dest => dest.Status, src => src.Status)
            .Map(dest => dest.Price, src => src.Price)
            // .TwoWays() se quita: solo se mapea de entidad a DTO, y los campos calculados no tienen camino de regreso

            // ----- NUEVO: relaciones -----
            // Category → CategoryListDto y Warehouse → WarehouseDto se mapean solos (DTOs anidados por convención).
            // Relación N:N: se omite la tabla intermedia ProductSupplier y se entrega la lista de proveedores
            .Map(dest => dest.Suppliers,
                 src => src.ProductSuppliers.Select(ps => ps.Supplier).OrderBy(s => s.SupplierName))

            // ----- NUEVO: campos calculados -----
            .Map(dest => dest.SupplierCount, src => src.ProductSuppliers.Count)
            .Map(dest => dest.StockStatus, src => src.StockQuantity < LowStockLimit ? "Bajo" : "Disponible");


        // =====================================================================
        // RETO: InvoiceItem → InvoiceItemDto (una línea)
        // =====================================================================
        TypeAdapterConfig<InvoiceItem, InvoiceItemDto>
            .NewConfig()
            // IdProduct, Price y Quantity se copian solos (mismo nombre)
            .Map(dest => dest.ProductName, src => src.Product.ProductName)
            .Map(dest => dest.Subtotal, src => LineSubtotal(src))
            .Map(dest => dest.Tax, src => LineTax(src))       // explícito: en la entidad, Tax es la relación
            .Map(dest => dest.Total, src => LineSubtotal(src) + LineTax(src));

        // =====================================================================
        // RETO: Invoice → InvoiceDetailDto (factura completa)
        // =====================================================================
        TypeAdapterConfig<Invoice, InvoiceDetailDto>
            .NewConfig()
            // IdInvoice e InvoiceDate se copian solos
            .Map(dest => dest.CustomerFullName,
                 src => src.Customer.FirstName + " " + src.Customer.Surname + " " + src.Customer.LastName)
            .Map(dest => dest.Items, src => src.InvoiceItems.OrderBy(ii => ii.LineNumber))   // usa el mapeo de arriba
            .Map(dest => dest.Subtotal, src => src.InvoiceItems.Sum(ii => LineSubtotal(ii)))
            .Map(dest => dest.Tax, src => src.InvoiceItems.Sum(ii => LineTax(ii)))
            .Map(dest => dest.Total, src => src.InvoiceItems.Sum(ii => LineSubtotal(ii) + LineTax(ii)));


        // =====================================================================
        // Supplier (CRUD)
        // Entidad → DTO de salida (SupplierListDto, SupplierDetailDto): mismos nombres,
        // Mapster los copia solo; no necesitan configuración.
        // DTO de entrada → entidad: se ignoran explícitamente los campos que el client envía.
         // =====================================================================
         TypeAdapterConfig<SupplierCreateDto, Supplier>
         .NewConfig()
         .Ignore(dest => dest.IdSupplier) // lo asigna el repositorio
         .Ignore(dest => dest.Status); // lo asigna el servicio: todo proveedor nuevo nace activo
 TypeAdapterConfig<SupplierUpdateDto, Supplier>
 .NewConfig()
 .Ignore(dest => dest.IdSupplier); // viene de la ruta, no del body
    }


}
