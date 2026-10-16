using System.ComponentModel.DataAnnotations;
namespace ERP.Domain.DTO;

public record class ProductSearchDto : IValidatableObject
{
    [StringLength(100, ErrorMessage = "El texto a buscar no puede superar 100 caracteres.")]
    public string? Name { get; set; }
    [Range(0, 999999.99, ErrorMessage = "El precio mínimo debe estar entre 0 y 999999.99.")]
    public decimal? MinPrice { get; set; }
    [Range(0, 999999.99, ErrorMessage = "El precio máximo debe estar entre 0 y 999999.99.")]
    public decimal? MaxPrice { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "La categoría no es válida.")]
    public int? IdCategory { get; set; }
    // Reglas que comparan VARIAS propiedades del mismo DTO.
    // ASP.NET Core solo llama a Validate si todos los atributos de las propiedades pasaron.
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (MinPrice.HasValue && MaxPrice.HasValue && MinPrice > MaxPrice)
        {
            yield return new ValidationResult(
            "El precio mínimo no puede ser mayor que el precio máximo.",
            new[] { nameof(MinPrice), nameof(MaxPrice) });
        }
        if (string.IsNullOrWhiteSpace(Name) && MinPrice is null && MaxPrice is null &&
       IdCategory is null)
        {
            yield return new ValidationResult(
            "Indique al menos un criterio de búsqueda: Name, MinPrice, MaxPrice o IdCategory.");
        }
    }
}
