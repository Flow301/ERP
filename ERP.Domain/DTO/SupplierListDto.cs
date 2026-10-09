namespace ERP.Domain.DTO;

public record class SupplierListDto
{
    public int IdSupplier { get; set; }
    public string? SupplierName { get; set; }
    public string? LegalEntityNumber { get; set; } // NUEVO
    public bool Status { get; set; } // NUEVO
}