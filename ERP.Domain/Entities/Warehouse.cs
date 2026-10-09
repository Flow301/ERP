namespace ERP.Domain.Entities;

public class Warehouse
{
    public int IdWarehouse { get; set; }

    public string WarehouseName { get; set; } = null!;

    public string? Address { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}