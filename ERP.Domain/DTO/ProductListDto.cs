namespace ERP.Domain.DTO;
/// <summary>
/// DTO de LISTA: liviano, para tablas. Se arma con una proyección (Select) en la consulta:
/// los campos calculados se resuelven en SQL.
/// </summary>
public record class ProductListDto
{
    public int IdProduct { get; set; }
    public string? ProductName { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string? CategoryName { get; set; } // JOIN con Category
    public string? WarehouseName { get; set; } // JOIN con Warehouse
    public decimal InventoryValue { get; set; } // calculado en SQL: Price * StockQuantity
}