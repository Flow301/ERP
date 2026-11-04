using ERP.Domain.Entities;
namespace ERP.Infraestructure.Interfaces;

public interface IInvoiceRepository
{
    // Consultas (laboratorio anterior) 
    Task<Invoice?> GetByIdAsync(int id);
    Task<IEnumerable<Invoice>> GetByCustomerAsync(string idCustomer);
    Task<IEnumerable<InvoiceItem>> GetLinesAsync(int idInvoice);

    // NUEVO  validaciones de integridad referencial 
    Task<bool> CustomerExistsAsync(string idCustomer);
    Task<bool> ActiveUserExistsAsync(int idUser);
    Task<bool> PaymentTypeExistsAsync(int idPaymentType);
    Task<bool> InvoiceStatusExistsAsync(int idInvoiceStatus);
    Task<bool> CreditCardExistsAsync(int idCreditCard);
    Task<IEnumerable<Product>> GetProductsAsync(IEnumerable<int> ids);
    Task<IEnumerable<Tax>> GetTaxesAsync(IEnumerable<int> ids);

    // NUEVO el proceso transaccional: devuelve el IdInvoice, o null si se revirtió 
    Task<int?> CreateAsync(Invoice invoice);
}
