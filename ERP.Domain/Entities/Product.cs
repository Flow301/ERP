namespace ERP.Domain.Entities;

public record class Product
{
    public int IdProduct { get; set; }
    public int IdWarehouse { get; set; }
    public int IdCategory { get; set; }
    public string ProductName { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string? Image { get; set; }
    public DateTime InsertDate { get; set; }
    public bool Status { get; set; }
    public virtual Warehouse Warehouse { get; set; } = null!;
    public virtual Category Category { get; set; } = null!;
    public virtual ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
    public virtual ICollection<ProductSupplier> ProductSuppliers { get; set; } = new List<ProductSupplier>();
}