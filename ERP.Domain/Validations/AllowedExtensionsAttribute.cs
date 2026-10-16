using System.ComponentModel.DataAnnotations;
namespace ERP.Domain.Validations;
/// <summary>
/// Atributo reutilizable: valida que un nombre de archivo termine en una de las extensiones permitidas.
/// Uso: [AllowedExtensions(".jpg", ".png")] sobre cualquier propiedad string de cualquier DTO.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class AllowedExtensionsAttribute : ValidationAttribute
{
    private readonly string[] _extensions;
    public AllowedExtensionsAttribute(params string[] extensions)
    {
        _extensions = extensions;
    }
    protected override ValidationResult? IsValid(object? value, ValidationContext
   validationContext)
    {
        // Un valor vacío no se valida aquí: eso es tarea de [Required]
        if (value is not string fileName || string.IsNullOrWhiteSpace(fileName))
            return ValidationResult.Success;
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        if (_extensions.Contains(extension))
            return ValidationResult.Success;
        return new ValidationResult(
        ErrorMessage ?? $"La imagen debe tener una de estas extensiones: {string.Join(", ",
       _extensions)}.",
        new[] { validationContext.MemberName! });
    }
}