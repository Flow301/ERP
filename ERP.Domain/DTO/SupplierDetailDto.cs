namespace ERP.Domain.DTO;

public record class SupplierDetailDto
{
    public int IdSupplier { get; set; }
    public string? SupplierName { get; set; }
    public string? LegalEntityNumber { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public bool Status { get; set; }
}