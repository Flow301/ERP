namespace ERP.Domain.Entities;

public record class Customer
{
    public string IdCustomer { get; set; } = null!;

    public int IdProvince { get; set; }

    public string FirstName { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string? LastName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Address { get; set; } = null!;

    // Navigation properties
    public virtual Province Province { get; set; } = null!;

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}
