using System.ComponentModel.DataAnnotations;
namespace ERP.Domain.DTO;

using ERP.Domain.Validations;

public record class ProductCreateDto
{
    [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
    [RegularExpression(@"^[^<>]*$", ErrorMessage = "No se permiten los caracteres < y >.")] // NUEVO
    public string? ProductName { get; set; }

    [Required(ErrorMessage = "La descripción es obligatoria.")]
    [StringLength(500, ErrorMessage = "La descripción no puede superar 500 caracteres.")]
    [RegularExpression(@"^[^<>]*$", ErrorMessage = "No se permiten los caracteres < y >.")] // NUEVO
    public string? Description { get; set; }

    [Range(0.01, 999999.99, ErrorMessage = "El precio debe estar entre 0.01 y 999999.99.")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "El inventario no puede ser negativo.")]
    public int StockQuantity { get; set; }

    //Pagina 10 lab 5.2
    [StringLength(200, ErrorMessage = "El nombre de la imagen no puede superar 200 caracteres.")]
    [AllowedExtensions(".jpg", ".jpeg", ".png", ".webp")]
    public string? Image { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La categoría es obligatoria.")]
    public int IdCategory { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La bodega es obligatoria.")]
    public int IdWarehouse { get; set; }

    public List<int> SupplierIds { get; set; } = new();
}