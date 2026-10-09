namespace ERP.Domain.DTO;

public record class WarehouseDto
{
    public int IdWarehouse { get; set; }
    public string? WarehouseName { get; set; }
    public string? Address { get; set; }
}