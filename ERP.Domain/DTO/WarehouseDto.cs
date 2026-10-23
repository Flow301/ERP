namespace ERP.Domain.DTO;

/// <summary>
/// Bodega (DTO de salida).
/// </summary>
public record class WarehouseDto
{
    /// <summary>Id de la bodega.</summary>
    /// <example>1</example>
    public int IdWarehouse { get; set; }
    /// <summary>Nombre de la bodega (único).</summary>
    /// <example>Bodega Central</example>
    public string? WarehouseName { get; set; }
    /// <summary>Dirección de la bodega.</summary>
    /// <example>San José Centro</example>
    public string? Address { get; set; }
}
