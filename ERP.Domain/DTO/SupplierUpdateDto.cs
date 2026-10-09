using System.ComponentModel.DataAnnotations;
namespace ERP.Domain.DTO;

public record class SupplierUpdateDto
{
    [Required, StringLength(100)]
    public string? SupplierName { get; set; }
    [Required, StringLength(20)]
    public string? LegalEntityNumber { get; set; }
    [StringLength(20)]
    public string? Phone { get; set; }
    [EmailAddress, StringLength(100)]
    public string? Email { get; set; }
    [StringLength(200)]
    public string? Address { get; set; }
    public bool Status { get; set; }
}
