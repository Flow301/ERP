using System.ComponentModel.DataAnnotations;
namespace ERP.Domain.DTO;

/// <summary>
/// Datos NUEVOS de una bodega. Sin IdWarehouse: viene de la ruta (PUT api/Warehouse/{id}), nunca del cuerpo.
/// </summary>
public record class WarehouseUpdateDto
{
    /// <summary>Nombre de la bodega: obligatorio, máximo 80 caracteres y no puede ser el de otra bodega.</summary>
    /// <example>Bodega Central</example>
    [Required(ErrorMessage = "El nombre de la bodega es obligatorio.")]
    [StringLength(80, ErrorMessage = "El nombre de la bodega no puede superar 80 caracteres.")]
    public string? WarehouseName { get; set; }
    /// <summary>Dirección de la bodega: opcional, máximo 120 caracteres.</summary>
    /// <example>San José Centro</example>
    [StringLength(120, ErrorMessage = "La dirección no puede superar 120 caracteres.")]
    public string? Address { get; set; }
}
