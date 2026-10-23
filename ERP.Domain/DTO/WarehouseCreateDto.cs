using System.ComponentModel.DataAnnotations;
namespace ERP.Domain.DTO;

/// <summary>
/// Datos para registrar una bodega. Sin IdWarehouse: lo asigna el servidor.
/// </summary>
// Los largos coinciden con las columnas de AppDbContext.
public record class WarehouseCreateDto
{
    /// <summary>Nombre de la bodega: obligatorio, máximo 80 caracteres y no se repite.</summary>
    /// <example>Bodega Norte Liberia</example>
    [Required(ErrorMessage = "El nombre de la bodega es obligatorio.")]
    [StringLength(80, ErrorMessage = "El nombre de la bodega no puede superar 80 caracteres.")]
    public string? WarehouseName { get; set; }
    /// <summary>Dirección de la bodega: opcional, máximo 120 caracteres.</summary>
    /// <example>Guanacaste, Liberia</example>
    [StringLength(120, ErrorMessage = "La dirección no puede superar 120 caracteres.")]
    public string? Address { get; set; }
}
