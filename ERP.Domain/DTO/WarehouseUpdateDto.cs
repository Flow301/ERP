using System.ComponentModel.DataAnnotations;
namespace ERP.Domain.DTO;

// Sin IdWarehouse: viene de la ruta (PUT api/Warehouse/{id}), nunca del body
public record class WarehouseUpdateDto
{
    [Required(ErrorMessage = "El nombre de la bodega es obligatorio.")]
    [StringLength(80, ErrorMessage = "El nombre de la bodega no puede superar 80 caracteres.")]
    public string? WarehouseName { get; set; }
    [StringLength(120, ErrorMessage = "La dirección no puede superar 120 caracteres.")]
    public string? Address { get; set; }
}
