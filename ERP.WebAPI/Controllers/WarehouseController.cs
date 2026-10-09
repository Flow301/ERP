using ERP.Application.Interfaces;
using ERP.Domain.DTO;
using ERP.Domain.DTO.PatternResult;
using Microsoft.AspNetCore.Mvc;
namespace ERP.WebAPI.Controllers;

// Los atributos ProducesResponseType documentan en Scalar cada código HTTP posible y la forma del JSON
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class WarehouseController : ControllerBase
{
    private readonly IWarehouseService _service;
    public WarehouseController(IWarehouseService service)
    {
        _service = service;
    }
    // GET api/Warehouse
    [HttpGet]
    [EndpointSummary("Lista todas las bodegas")]
    [ProducesResponseType<Result<IEnumerable<WarehouseDto>>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _service.GetAllAsync();
        return StatusCode(result.Status, result);
    }
    // GET api/Warehouse/{pageNumber}/{pageSize}
    [HttpGet("{pageNumber:int}/{pageSize:int}")]
    [EndpointSummary("Lista las bodegas por página")]
    [ProducesResponseType<Result<IEnumerable<WarehouseDto>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Result<IEnumerable<WarehouseDto>>>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPaginationAsync(int pageNumber = 1, int pageSize = 10)
    {
        var result = await _service.GetPaginationAsync(pageNumber, pageSize);
        return StatusCode(result.Status, result);
    }
    // GET api/Warehouse/{id}
    [HttpGet("{id:int}", Name = "GetWarehouseById")]
    [EndpointSummary("Obtiene una bodega por id")]
    [ProducesResponseType<Result<WarehouseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Result<WarehouseDto>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return StatusCode(result.Status, result);
    }
    // POST api/Warehouse
    [HttpPost]
    [EndpointSummary("Crea una bodega")]
    [ProducesResponseType<Result<WarehouseDto>>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<Result<WarehouseDto>>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddAsync([FromBody] WarehouseCreateDto warehouse)
    {
        var result = await _service.AddAsync(warehouse);
        if (!result.IsSuccess)
            return StatusCode(result.Status, result);
        // 201 Created + encabezado Location: /api/Warehouse/{id}
        return CreatedAtRoute("GetWarehouseById", new { id = result.Data!.IdWarehouse }, result);
    }
    // PUT api/Warehouse/{id}
    [HttpPut("{id:int}")]
    [EndpointSummary("Actualiza una bodega")]
    [ProducesResponseType<Result<WarehouseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<Result<WarehouseDto>>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<Result<WarehouseDto>>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] WarehouseUpdateDto warehouse)
    {
        var result = await _service.UpdateAsync(id, warehouse);
        return StatusCode(result.Status, result);
    }
    // DELETE api/Warehouse/{id}
    [HttpDelete("{id:int}")]
    [EndpointSummary("Elimina una bodega sin productos")]
    [ProducesResponseType<Result<int>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Result<int>>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<Result<int>>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var result = await _service.DeleteAsync(id);
        return StatusCode(result.Status, result);
    }
}
