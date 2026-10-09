namespace ERP.Domain.DTO;
/// <summary>
/// DTO de ESTADÍSTICAS: resultado de un GroupBy. Todos sus valores los calcula SQL Server.
/// </summary>
public record class CategoryStatsDto
{
    public int IdCategory { get; set; }
    public string? CategoryName { get; set; }
    public int ProductCount { get; set; }
    public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }
    public decimal AveragePrice { get; set; }
    public decimal InventoryValue { get; set; }
}