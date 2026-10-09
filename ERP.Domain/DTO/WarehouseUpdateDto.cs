using System.ComponentModel.DataAnnotations;
namespace ERP.Domain.DTO;

// Sin IdWarehouse: viene de la ruta (PUT api/Warehouse/{id}), nunca del body
public record class WarehouseUpdateDto
{
    [Required, StringLength(80)]
    public string? WarehouseName { get; set; }
    [StringLength(120)]
    public string? Address { get; set; }
}
