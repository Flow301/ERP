using ERP.Domain.DTO;
using ERP.Domain.DTO.PatternResult;

namespace ERP.Application.Interfaces;

public interface IInvoiceService
{
    // Consultas: ADAPTADAS a Result<T> 
    Task<Result<InvoiceDetailDto>> GetByIdAsync(int id);
    Task<Result<IEnumerable<InvoiceDetailDto>>> GetByCustomerAsync(string idCustomer);
    Task<Result<IEnumerable<InvoiceItemDto>>> GetLinesAsync(int idInvoice);

    // NUEVO  proceso de negocio 
    Task<Result<InvoiceDetailDto>> RegisterAsync(InvoiceCreateDto invoice);

    // RETO  anular una factura 
    Task<Result<InvoiceDetailDto>> VoidAsync(int id);
}