namespace ERP.Domain.DTO;
/// <summary>
/// /// DTO de DETALLE (un solo producto). Mantiene los campos del laboratorio anterior
/// y agrega las relaciones como DTOs anidados y los campos calculados por el mapper.
/// </summary>
public record class ProductDetailDto
{
    public int IdProduct { get; set; }
    public string? ProductName { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string? Image { get; set; }
    public DateTime InsertDate { get; set; }
    public bool Status { get; set; }
    // NUEVO: DTOs anidados (se llenan con Include/ThenInclude + Mapster)
    public CategoryListDto? Category { get; set; } // relación N:1
    public WarehouseDto? Warehouse { get; set; } // relación N:1
    public List<SupplierListDto> Suppliers { get; set; } = new(); // relación N:N
                                                                  // NUEVO: campos calculados por el mapper
    public int SupplierCount { get; set; }
    public string? StockStatus { get; set; }
}
