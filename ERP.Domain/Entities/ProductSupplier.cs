namespace ERP.Domain.Entities;

public record class ProductSupplier
{
    public int IdProduct { get; set; }
    public int IdSupplier { get; set; }
    public virtual Product Product { get; set; } = null!;
    public virtual Supplier Supplier { get; set; } = null!;
}
