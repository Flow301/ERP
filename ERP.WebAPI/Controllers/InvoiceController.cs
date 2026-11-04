using ERP.Application.Interfaces;
using ERP.Domain.DTO;
using ERP.Domain.DTO.PatternResult;
using Microsoft.AspNetCore.Mvc;

namespace ERP.WebAPI.Controllers;

/// <summary> 
/// Facturación: consulta de facturas y registro de ventas. 
/// </summary> 
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
[ProducesResponseType<Result<object>>(500)]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceService _service;

    public InvoiceController(IInvoiceService service)
    {
        _service = service;
    }

    /// <summary> 
    /// Obtiene una factura con su cliente, líneas y totales. 
    /// </summary> 
    /// <param name="id" example="1">Número de factura.</param> 
    /// <response code="200">Factura encontrada.</response> 
    /// <response code="404">La factura no existe.</response> 
    [HttpGet("{id:int}", Name = "GetInvoiceById")]
    [ProducesResponseType<Result<InvoiceDetailDto>>(200)]
    [ProducesResponseType<Result<object>>(404)]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var result = await _service.GetByIdAsync(id);

        return StatusCode(result.Status, result);
    }

    /// <summary> 
    /// Lista las facturas de un cliente. 
    /// </summary> 
    /// <remarks>Un cliente sin facturas responde 200 con una lista vacía.</remarks> 
    /// <param name="idCustomer" example="101110111">Cédula del cliente.</param> 
    /// <response code="200">Facturas del cliente (puede ser una lista vacía).</response> 
    [HttpGet("by-customer/{idCustomer}")]
    [ProducesResponseType<Result<IEnumerable<InvoiceDetailDto>>>(200)]
    public async Task<IActionResult> GetByCustomerAsync(string idCustomer)
    {
        var result = await _service.GetByCustomerAsync(idCustomer);

        return StatusCode(result.Status, result);
    }
    /// <summary> 
    /// Obtiene solo las líneas de una factura. 
    /// </summary> 
    /// <param name="id" example="1">Número de factura.</param> 
    /// <response code="200">Líneas de la factura.</response> 
    /// <response code="404">La factura no existe.</response> 
    [HttpGet("{id:int}/lines")]
    [ProducesResponseType<Result<IEnumerable<InvoiceItemDto>>>(200)]
    [ProducesResponseType<Result<object>>(404)]
    public async Task<IActionResult> GetLinesAsync(int id)
    {
        var result = await _service.GetLinesAsync(id);

        return StatusCode(result.Status, result);
    }

    /// <summary> 
    /// Registra una factura completa (encabezado y líneas) como una sola operación. 
    /// </summary> 
    /// <remarks> 
    /// El precio de cada línea se toma del catálogo y el inventario se rebaja en la misma transacción.
    /// Si una regla falla, no se guarda nada. Pagos con tarjeta (2 y 3) exigen IdCreditCard y los últimos 4 dígitos.
    /// </remarks> 
    /// <param name="invoice">Encabezado y líneas de la venta.</param> 
    /// <response code="201">Factura registrada; el encabezado Location apunta a GET api/Invoice/{id}.</response> 
    /// <response code="400">Datos inválidos o referencias que no existen (cliente, usuario, producto…).</response> 
    /// <response code="409">Inventario insuficiente; no se guardó nada.</response> 
    [HttpPost]
    [ProducesResponseType<Result<InvoiceDetailDto>>(201)]
    [ProducesResponseType<Result<Dictionary<string, string[]>>>(400)]
    [ProducesResponseType<Result<object>>(409)]
    public async Task<IActionResult> RegisterAsync([FromBody] InvoiceCreateDto invoice)
    {
        var result = await _service.RegisterAsync(invoice);

        if (!result.IsSuccess)
            return StatusCode(result.Status, result);

        // 201 Created + encabezado Location: /api/Invoice/{id} 
        return CreatedAtRoute("GetInvoiceById", new { id = result.Data!.IdInvoice }, result);
    }
}


