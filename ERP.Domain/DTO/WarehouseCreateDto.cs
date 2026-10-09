using System.ComponentModel.DataAnnotations;
namespace ERP.Domain.DTO;

// Sin IdWarehouse: lo asigna el repositorio. Los largos coinciden con las columnas de AppDbContext.
public record class WarehouseCreateDto
{
    [Required, StringLength(80)]
    public string? WarehouseName { get; set; }
    [StringLength(120)]
    public string? Address { get; set; }
}
