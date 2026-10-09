using ERP.Application.Interfaces;
using ERP.Domain.DTO;
using ERP.Domain.DTO.PatternResult;
using ERP.Infraestructure.Interfaces;
using MapsterMapper;

namespace ERP.Application.Implementation;

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _repository;
    private readonly IMapper _mapper;

    public InvoiceService(IInvoiceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    // Detalle: el mapper arma las líneas y los montos calculados
    public async Task<Result<InvoiceDetailDto>> GetByIdAsync(int id)
    {
        var invoice = await _repository.GetByIdAsync(id);

        if (invoice is null)
            return Result<InvoiceDetailDto>.Failure(404, "Not Found", $"Invoice {id} doesn't exist!");

        var data = _mapper.Map<InvoiceDetailDto>(invoice);

        var result = Result<InvoiceDetailDto>.Success(data);

        return result;
    }

    // Cliente sin facturas → 200 con [] (no es un error)
    public async Task<Result<IEnumerable<InvoiceDetailDto>>> GetByCustomerAsync(string idCustomer)
    {
        var invoices = await _repository.GetByCustomerAsync(idCustomer.Trim());

        var data = _mapper.Map<IEnumerable<InvoiceDetailDto>>(invoices);

        var result = Result<IEnumerable<InvoiceDetailDto>>.Success(data);

        return result;
    }

    // Toda factura tiene al menos una línea: sin líneas significa que la factura no existe
    public async Task<Result<IEnumerable<InvoiceItemDto>>> GetLinesAsync(int idInvoice)
    {
        var items = await _repository.GetLinesAsync(idInvoice);

        if (!items.Any())
            return Result<IEnumerable<InvoiceItemDto>>.Failure(404, "Not Found", $"Invoice {idInvoice} doesn't exist!");

        var data = _mapper.Map<IEnumerable<InvoiceItemDto>>(items);

        var result = Result<IEnumerable<InvoiceItemDto>>.Success(data);

        return result;
    }
}
