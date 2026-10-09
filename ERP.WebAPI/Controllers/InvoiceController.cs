using ERP.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace ERP.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceService _service;
    public InvoiceController(IInvoiceService service)
    {
        _service = service;
    }
    // GET api/Invoice/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return StatusCode(result.Status, result);
    }
    // GET api/Invoice/by-customer/{idCustomer}
    [HttpGet("by-customer/{idCustomer}")]
    [HttpGet("bycustomer/{idCustomer}")]
    public async Task<IActionResult> GetByCustomerAsync(string idCustomer)
    {
        var result = await _service.GetByCustomerAsync(idCustomer);
        return StatusCode(result.Status, result);
    }
    // GET api/Invoice/{id}/lines
    [HttpGet("{id:int}/lines")]
    public async Task<IActionResult> GetLinesAsync(int id)
    {
        var result = await _service.GetLinesAsync(id);
        return StatusCode(result.Status, result);
    }
}
