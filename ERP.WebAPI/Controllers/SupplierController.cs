using ERP.Application.Interfaces;
using ERP.Domain.DTO;
using Microsoft.AspNetCore.Mvc;
namespace ERP.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SupplierController : ControllerBase
{
    private readonly ISupplierService _service;
    public SupplierController(ISupplierService service)
    {
        _service = service;
    }
    // GET api/Supplier
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _service.GetAllAsync();
        return StatusCode(result.Status, result);
    }
    // GET api/Supplier/{pageNumber}/{pageSize}
    [HttpGet("{pageNumber:int}/{pageSize:int}")]
    public async Task<IActionResult> GetPaginationAsync(int pageNumber = 1, int pageSize =
   10)
    {
        var result = await _service.GetPaginationAsync(pageNumber, pageSize);
        return StatusCode(result.Status, result);
    }
    // GET api/Supplier/{id}
    [HttpGet("{id:int}", Name = "GetSupplierById")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return StatusCode(result.Status, result);
    }
    // POST api/Supplier
    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] SupplierCreateDto supplier)
    {
        var result = await _service.AddAsync(supplier);
        if (!result.IsSuccess)
            return StatusCode(result.Status, result);
        // 201 Created + encabezado Location: /api/Supplier/{id}
        return CreatedAtRoute("GetSupplierById", new { id = result.Data!.IdSupplier }, result);
    }
    // PUT api/Supplier/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody]
SupplierUpdateDto supplier)
    {
        var result = await _service.UpdateAsync(id, supplier);
        return StatusCode(result.Status, result);
    }
    // DELETE api/Supplier/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var result = await _service.DeleteAsync(id);
        return StatusCode(result.Status, result);
    }
}
