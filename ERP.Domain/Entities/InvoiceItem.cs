namespace ERP.Domain.Entities;

public record class InvoiceItem
{
    public int IdInvoice { get; set; }
    public int LineNumber { get; set; }
    public int IdProduct { get; set; }
    public int IdTax { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public virtual Invoice Invoice { get; set; } = null!;
    public virtual Product Product { get; set; } = null!;
    public virtual Tax Tax { get; set; } = null!;
}
