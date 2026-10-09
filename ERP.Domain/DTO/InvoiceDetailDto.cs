namespace ERP.Domain.DTO;
/// <summary>
/// Factura completa: encabezado, cliente y líneas. Los montos son la suma de las líneas.
/// </summary>
public record class InvoiceDetailDto
{
    public int IdInvoice { get; set; }
    public DateTime InvoiceDate { get; set; }
    public string? CustomerFullName { get; set; }
    public List<InvoiceItemDto> Items { get; set; } = new();
    // Campos calculados por el mapper
    public decimal Subtotal { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }
}
