namespace ERP.Domain.Entities; 
public record class PaymentType { 
    public int IdPaymentType { get; set; } 
    public string PaymentTypeDescription { get; set; } = null!; 
    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>(); 
}
