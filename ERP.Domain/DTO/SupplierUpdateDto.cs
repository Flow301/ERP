using System.ComponentModel.DataAnnotations;
namespace ERP.Domain.DTO;

public record class SupplierUpdateDto
{
    [Required, StringLength(100)]
    [RegularExpression(@"^[^<>]*$", ErrorMessage = "No se permiten los caracteres < y >.")]
    public string? SupplierName { get; set; }
    [Required, StringLength(20)]
    public string? LegalEntityNumber { get; set; }
    [StringLength(20)]
    public string? Phone { get; set; }
    [EmailAddress, StringLength(100)]
    public string? Email { get; set; }
    [StringLength(200)]
    [RegularExpression(@"^[^<>]*$", ErrorMessage = "No se permiten los caracteres < y >.")]
    public string? Address { get; set; }
    public bool Status { get; set; }
}
