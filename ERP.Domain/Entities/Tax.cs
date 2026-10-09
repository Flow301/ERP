namespace ERP.Domain.Entities;

public record class Tax
{
    public int IdTax { get; set; }
    public string TaxName { get; set; } = null!;
    public decimal Percentage { get; set; }
    public virtual ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
}
