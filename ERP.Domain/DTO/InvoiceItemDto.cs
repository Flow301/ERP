namespace ERP.Domain.DTO;
/// <summary>
/// Una línea de factura. Subtotal, Tax y Total los calcula el mapper (redondeados a 2 decimales).
/// </summary>
public record class InvoiceItemDto
{
    public int IdProduct { get; set; }
    public string? ProductName { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    // Campos calculados por el mapper
    public decimal Subtotal { get; set; } // Price × Quantity
    public decimal Tax { get; set; }      // Subtotal × Percentage / 100
    public decimal Total { get; set; }    // Subtotal + Tax
}
