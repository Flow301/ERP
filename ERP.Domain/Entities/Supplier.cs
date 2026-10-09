namespace ERP.Domain.Entities;

public record class Supplier
{
    public int IdSupplier { get; set; }
    public string SupplierName { get; set; } = null!;
    public string? LegalEntityNumber { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public bool Status { get; set; }
    public virtual ICollection<ProductSupplier> ProductSuppliers { get; set; } = new List<ProductSupplier>();
}
