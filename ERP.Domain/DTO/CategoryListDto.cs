namespace ERP.Domain.DTO;

public record class CategoryListDto
{
    public int IdCategory { get; set; }
    public string? CategoryName { get; set; }
}