using ERP.Domain.Entities;
namespace ERP.Infraestructure.Interfaces;

public interface IInvoiceRepository
{
    Task<Invoice?> GetByIdAsync(int id);
    Task<IEnumerable<Invoice>> GetByCustomerAsync(string idCustomer);
    Task<IEnumerable<InvoiceItem>> GetLinesAsync(int idInvoice);
}
