namespace ERP.Domain.DTO;

/// <summary>
/// DTO de una LÍNEA de factura. Subtotal, Tax y Total son campos calculados por el mapper.
/// </summary>
public record class InvoiceItemDto
{
    public int LineNumber { get; set; } // NUEVO 7-1
    public int IdProduct { get; set; }
    public string? ProductName { get; set; } // de InvoiceItem.Product (Include)
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    // Campos calculados (MapsterConfig)
    public decimal Subtotal { get; set; } // Price × Quantity
    public decimal Tax { get; set; } // Subtotal × Percentage / 100
    public decimal Total { get; set; } // Subtotal + Tax
}
