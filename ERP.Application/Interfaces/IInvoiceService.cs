using ERP.Domain.DTO;
using ERP.Domain.DTO.PatternResult;
namespace ERP.Application.Interfaces;

public interface IInvoiceService
{
    Task<Result<InvoiceDetailDto>> GetByIdAsync(int id);
    Task<Result<IEnumerable<InvoiceDetailDto>>> GetByCustomerAsync(string idCustomer);
    Task<Result<IEnumerable<InvoiceItemDto>>> GetLinesAsync(int idInvoice);
}
