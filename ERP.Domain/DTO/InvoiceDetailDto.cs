namespace ERP.Domain.DTO;

/// <summary>
/// DTO de DETALLE de una factura: encabezado, cliente, líneas y totales.
/// </summary>
public record class InvoiceDetailDto
{
    public int IdInvoice { get; set; }
    public DateTime InvoiceDate { get; set; }

    // NUEVO datos del encabezado (se copian solos: mismo nombre que en Invoice)
    public string? IdCustomer { get; set; }
    public int IdUser { get; set; }
    public int IdPaymentType { get; set; }
    public int IdInvoiceStatus { get; set; }
    public int? IdCreditCard { get; set; }
    public string? CreditCardNumber { get; set; }

    public string? CustomerFullName { get; set; }      // de Invoice.Customer (Include)

    public List<InvoiceItemDto> Items { get; set; } = new(); // DTO anidado (relación 1:N)

    // Campos calculados (MapsterConfig): suma de las líneas
    public decimal Subtotal { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }
}
