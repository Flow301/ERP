namespace ERP.Domain.Entities;

public record class Invoice
{
    public int IdInvoice { get; set; }

    public string IdCustomer { get; set; } = null!;

    public int IdUser { get; set; }

    public int IdInvoiceStatus { get; set; }

    public int IdPaymentType { get; set; }

    public int? IdCreditCard { get; set; }

    public string? CreditCardNumber { get; set; }

    public DateTime InvoiceDate { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual InvoiceStatus InvoiceStatus { get; set; } = null!;

    public virtual PaymentType PaymentType { get; set; } = null!;

    public virtual CreditCard? CreditCard { get; set; }

    public virtual ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
}