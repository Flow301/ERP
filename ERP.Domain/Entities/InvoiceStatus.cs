namespace ERP.Domain.Entities;

public record class InvoiceStatus
{
    public int IdInvoiceStatus { get; set; }

    public string InvoiceStatusDescription { get; set; } = null!;

    // Navigation properties
    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}