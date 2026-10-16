using System.ComponentModel.DataAnnotations;
namespace ERP.Domain.DTO;

// Sin IdWarehouse: lo asigna el repositorio. Los largos coinciden con las columnas de AppDbContext.
public record class WarehouseCreateDto
{
    [Required(ErrorMessage = "El nombre de la bodega es obligatorio.")]
    [StringLength(80, ErrorMessage = "El nombre de la bodega no puede superar 80 caracteres.")]
    public string? WarehouseName { get; set; }
    [StringLength(120, ErrorMessage = "La dirección no puede superar 120 caracteres.")]
    public string? Address { get; set; }
}
