using ERP.Domain.DTO;
using ERP.Domain.DTO.PatternResult;

namespace ERP.Application.Interfaces;

public interface IInvoiceService
{
    // Consultas: ADAPTADAS a Result<T>
    Task<Result<InvoiceDetailDto>> GetByIdAsync(int id, int idUser, bool isAdmin);   // RETO 9-2
    Task<Result<IEnumerable<InvoiceDetailDto>>> GetByCustomerAsync(string idCustomer);
    Task<Result<IEnumerable<InvoiceItemDto>>> GetLinesAsync(int idInvoice);

    // NUEVO proceso de negocio
    // El IdUser lo pasa el controller, leído del token
    Task<Result<InvoiceDetailDto>> RegisterAsync(InvoiceCreateDto invoice, int idUser);

    // RETO anular factura
    Task<Result<InvoiceDetailDto>> VoidAsync(int id);
}
