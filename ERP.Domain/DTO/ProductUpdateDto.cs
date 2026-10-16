using System.ComponentModel.DataAnnotations;
namespace ERP.Domain.DTO;

using ERP.Domain.Validations;


public record class ProductUpdateDto
{
    [Required, StringLength(100)]
    public string? ProductName { get; set; }
    [Required, StringLength(500)]
    public string? Description { get; set; }
    [Range(0.01, 999999.99)]
    public decimal Price { get; set; }
    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }
    [StringLength(200, ErrorMessage = "El nombre de la imagen no puede superar 200 caracteres.")]
 [AllowedExtensions(".jpg", ".jpeg", ".png", ".webp")]
    public string? Image { get; set; }
    [Range(1, int.MaxValue)]
    public int IdCategory { get; set; }
    [Range(1, int.MaxValue)]
    public int IdWarehouse { get; set; }
    public List<int> SupplierIds { get; set; } = new();
}