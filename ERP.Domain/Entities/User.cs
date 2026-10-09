namespace ERP.Domain.Entities;

public record class User
{
    public int IdUser { get; set; }

    public int IdRole { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool Status { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}
