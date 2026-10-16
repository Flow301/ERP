using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
namespace ERP.Domain.DTO;
using ERP.Domain.Validations; 
public record class SupplierCreateDto
{
    [Required(ErrorMessage = "El nombre del proveedor es obligatorio.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
 public string? SupplierName { get; set; }
    [Required(ErrorMessage = "La cédula jurídica es obligatoria.")]
    [RegularExpression(@"^\d-\d{3}-\d{6}$", ErrorMessage = "La cédula jurídica debe tener el formato 3-101-123456.")]
 public string? LegalEntityNumber { get; set; }
    [RegularExpression(@"^\d{4}-?\d{4}$", ErrorMessage = "El teléfono debe tener 8 dígitos, con o sin guion: 2201-1001.")]
 public string? Phone { get; set; }
    [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
    [StringLength(100, ErrorMessage = "El correo no puede superar 100 caracteres.")]
    public string? Email { get; set; }
    [StringLength(200, ErrorMessage = "La dirección no puede superar 200 caracteres.")]
    public string? Address { get; set; }
}
