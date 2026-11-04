using System.Globalization;
using ERP.Application.Interfaces;
using ERP.Domain.DTO;
using ERP.Domain.DTO.PatternResult;
using ERP.Domain.Entities;
using ERP.Infraestructure.Interfaces;
using MapsterMapper;
using Microsoft.Extensions.Logging;
namespace ERP.Application.Implementation;

public class InvoiceService : IInvoiceService
{
    // Regla de negocio: una factura no puede nacer anulada (3 = Anulada en CatalogSeeder) 
    public const int StatusVoided = 3;

    private readonly IInvoiceRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<InvoiceService> _logger;          // NUEVO 

    public InvoiceService(IInvoiceRepository repository, IMapper mapper,
ILogger<InvoiceService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    // ===================================================================== 
    // CONSULTAS (ADAPTADAS: ahora devuelven Result<T>) 
    // ===================================================================== 

    public async Task<Result<InvoiceDetailDto>> GetByIdAsync(int id)
    {
        var invoice = await _repository.GetByIdAsync(id);

        if (invoice is null)
            return Result<InvoiceDetailDto>.Failure(404, "Not Found", $"Invoice {id} doesn't exist!");
        var data = _mapper.Map<InvoiceDetailDto>(invoice);

        var result = Result<InvoiceDetailDto>.Success(data);

        return result;
    }

    public async Task<Result<IEnumerable<InvoiceDetailDto>>> GetByCustomerAsync(string
idCustomer)
    {
        var invoices = await _repository.GetByCustomerAsync(idCustomer.Trim());

        var data = _mapper.Map<IEnumerable<InvoiceDetailDto>>(invoices);

        // Cliente sin facturas → 200 con [] (no es un error) 
        var result = Result<IEnumerable<InvoiceDetailDto>>.Success(data);

        return result;
    }

    public async Task<Result<IEnumerable<InvoiceItemDto>>> GetLinesAsync(int idInvoice)
    {
        var lines = await _repository.GetLinesAsync(idInvoice);

        if (!lines.Any())
            return Result<IEnumerable<InvoiceItemDto>>.Failure(404, "Not Found", $"Invoice {idInvoice}doesn't exist!");


        var data = _mapper.Map<IEnumerable<InvoiceItemDto>>(lines);

        var result = Result<IEnumerable<InvoiceItemDto>>.Success(data);
        return result;
    }

    // ===================================================================== 
    // NUEVO  PROCESO DE NEGOCIO: REGISTRAR FACTURA 
    // ===================================================================== 

    public async Task<Result<InvoiceDetailDto>> RegisterAsync(InvoiceCreateDto invoice)
    {
        var idCustomer = invoice.IdCustomer!.Trim();

        // ---------- 1. Reglas de negocio e integridad referencial (antes de tocar datos) ---------- 

        if (!await _repository.CustomerExistsAsync(idCustomer))
            return Result<InvoiceDetailDto>.Failure(400, "Bad Request", $"Customer {idCustomer} doesn't exist!");


        if (!await _repository.ActiveUserExistsAsync(invoice.IdUser))
            return Result<InvoiceDetailDto>.Failure(400, "Bad Request", $"User {invoice.IdUser} doesn't exist or is inactive!");


        if (!await _repository.PaymentTypeExistsAsync(invoice.IdPaymentType))
            return Result<InvoiceDetailDto>.Failure(400, "Bad Request", $"Payment type {invoice.IdPaymentType}doesn't exist!");


        if (invoice.IdInvoiceStatus == StatusVoided)
            return Result<InvoiceDetailDto>.Failure(400, "Bad Request", "An invoice can't be created as voided!");


        if (!await _repository.InvoiceStatusExistsAsync(invoice.IdInvoiceStatus))
            return Result<InvoiceDetailDto>.Failure(400, "Bad Request", $"Invoice status {invoice.IdInvoiceStatus}doesn't exist!");

        // El DTO ya garantizó que la tarjeta viene solo cuando el pago es con tarjeta 
        if (invoice.IdCreditCard is not null &&
            !await _repository.CreditCardExistsAsync(invoice.IdCreditCard.Value))
            return Result<InvoiceDetailDto>.Failure(400, "Bad Request", $"Credit card {invoice.IdCreditCard}doesn't exist!");

        // Productos: una sola consulta para todas las líneas 
        var productIds = invoice.Items.Select(i => i.IdProduct).ToList();
        var products = (await _repository.GetProductsAsync(productIds)).ToDictionary(p => p.IdProduct);

        var unavailable = productIds
            .Where(id => !products.ContainsKey(id) || !products[id].Status)
            .ToList();

        if (unavailable.Any())
            return Result<InvoiceDetailDto>.Failure(400, "Bad Request",
                $"Products not available: {string.Join(", ", unavailable)}!");

        var shortage = invoice.Items.FirstOrDefault(i => products[i.IdProduct].StockQuantity < i.Quantity);

        if (shortage is not null)
        {
            var product = products[shortage.IdProduct];
            return Result<InvoiceDetailDto>.Failure(409, "Conflict",
                $"Insufficient stock for {product.ProductName}: available {product.StockQuantity}, requested {shortage.Quantity}!");
            // Impuestos de las líneas 
            var taxIds = invoice.Items.Select(i => i.IdTax).Distinct().ToList();
            var taxes = (await _repository.GetTaxesAsync(taxIds)).ToDictionary(t => t.IdTax);

            var missingTaxes = taxIds.Where(id => !taxes.ContainsKey(id)).ToList();

            if (missingTaxes.Any())
                return Result<InvoiceDetailDto>.Failure(400, "Bad Request",
                    $"Taxes don't exist: {string.Join(", ", missingTaxes)}!");

            // ---------- 2. Armar la factura en memoria ---------- 

            var entity = _mapper.Map<Invoice>(invoice);         // encabezado: los campos con el mismo nombre
            entity.IdCustomer = idCustomer;
            entity.InvoiceDate = DateTime.Now;                  // la fecha la pone el servidor, nunca el cliente

            var lineNumber = 1;
            foreach (var line in invoice.Items)
            {
                entity.InvoiceItems.Add(new InvoiceItem
                {
                    LineNumber = lineNumber++,
                    IdProduct = line.IdProduct,
                    IdTax = line.IdTax,
                    Quantity = line.Quantity,
                    Price = products[line.IdProduct].Price      // precio CONGELADO: el del catálogo hoy 
                });
            }
            // ---------- 3. Guardar todo como una única operación lógica ---------- 

            var idInvoice = await _repository.CreateAsync(entity);

            if (idInvoice is null)
            {
                _logger.LogWarning("Factura revertida para el cliente {IdCustomer}: el inventario cambió durante el registro",

                    idCustomer);

                return Result<InvoiceDetailDto>.Failure(409, "Conflict",
                    "The stock changed while saving. Nothing was saved, please try again!");
            }

            // ---------- 4. Leer lo guardado: subtotal, IVA y total ---------- 
            // Los totales NO se guardan: son los campos calculados de MapsterConfig 
            // (LineSubtotal y LineTax), los mismos que usa GET api/Invoice/{id}. 

            var saved = await _repository.GetByIdAsync(idInvoice.Value);

            var data = _mapper.Map<InvoiceDetailDto>(saved!);

            _logger.LogInformation("Factura {IdInvoice} registrada: {Lines} líneas, subtotal { Subtotal}, IVA { Tax}, total { Total}",
            idInvoice, data.Items.Count, data.Subtotal, data.Tax, data.Total);

            // ---------- 5. Responder ---------- 

            var detail = string.Create(CultureInfo.InvariantCulture, $"Invoice {idInvoice} saved! Total: {data.Total:0.00}");
            var result = Result<InvoiceDetailDto>.Success(data, 201, "Created", detail);

            return result;
        }
    }
}




