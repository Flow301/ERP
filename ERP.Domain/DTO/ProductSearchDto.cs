using System.ComponentModel.DataAnnotations;
namespace ERP.Domain.DTO;

/// <summary>
/// Parámetros de consulta de GET api/Product/search: filtros, búsqueda, ordenamiento y paginación.
/// Todos son opcionales: sin parámetros devuelve la primera página de productos activos ordenados por nombre.
/// </summary>
public record class ProductSearchDto : IValidatableObject
{
    // ----- Búsqueda -----
    /// <summary>Texto que debe contener el nombre del producto.</summary>
    /// <example>camisa</example>
    [StringLength(100, ErrorMessage = "El texto a buscar no puede superar 100 caracteres.")]
    public string? Name { get; set; }
    // ----- Filtros -----
    /// <summary>Precio mínimo (inclusive).</summary>
    /// <example>20</example>
    [Range(0, 999999.99, ErrorMessage = "El precio mínimo debe estar entre 0 y 999999.99.")]
    public decimal? MinPrice { get; set; }
    /// <summary>Precio máximo (inclusive).</summary>
    /// <example>100</example>
    [Range(0, 999999.99, ErrorMessage = "El precio máximo debe estar entre 0 y 999999.99.")]
    public decimal? MaxPrice { get; set; }
    /// <summary>Id de la categoría.</summary>
    /// <example>1</example>
    [Range(1, int.MaxValue, ErrorMessage = "La categoría no es válida.")]
    public int? IdCategory { get; set; }
    /// <summary>NUEVO — Id de un proveedor que surte el producto.</summary>
    /// <example>1</example>
    [Range(1, int.MaxValue, ErrorMessage = "El proveedor no es válido.")]
    public int? IdSupplier { get; set; }
    /// <summary>NUEVO — Estado: true = activos (por defecto), false =desactivados.</summary>
 /// <example>true</example>
 public bool Status { get; set; } = true;
    // ----- Ordenamiento -----
    /// <summary>NUEVO — Campo de orden: name, price, stock o date.</summary>
    /// <example>price</example>
    [RegularExpression("^(?i)(name|price|stock|date)$",
    ErrorMessage = "SortBy solo admite: name, price, stock o date.")]
    public string? SortBy { get; set; } = "name";
    /// <summary>NUEVO — Dirección del orden: asc o desc.</summary>
    /// <example>desc</example>
    [RegularExpression("^(?i)(asc|desc)$", ErrorMessage = "SortDirection solo admite: asc desc.")]
 public string? SortDirection { get; set; } = "asc";
    // ----- Paginación -----
    /// <summary>NUEVO — Número de página (desde 1).</summary>
    /// <example>1</example>
    [Range(1, int.MaxValue, ErrorMessage = "PageNumber debe ser 1 o más.")]
    public int PageNumber { get; set; } = 1;
    /// <summary>NUEVO — Registros por página (1 a 50).</summary>
    /// <example>10</example>
    [Range(1, 50, ErrorMessage = "PageSize debe estar entre 1 y 50.")]
    public int PageSize { get; set; } = 10;
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
        // 6-2: se quitó la regla "al menos un criterio". Con paginación,
        // una búsqueda sin filtros es válida: devuelve la primera página del catálogo.
    }
}
    
 