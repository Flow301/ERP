using ERP.Application.Interfaces;
using ERP.Domain.DTO;
using ERP.Domain.DTO.PatternResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.WebAPI.Controllers;

/// <summary>
/// Bodegas: consulta, registro, actualización y eliminación.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
[Authorize(Policy = AuthorizationPolicies.AdminOnly)]   // solo Administrador
[ProducesResponseType<Result<object>>(401)]
[ProducesResponseType<Result<object>>(403)]
[ProducesResponseType<Result<object>>(StatusCodes.Status500InternalServerError)]   // aplica a TODAS las acciones
public class WarehouseController : ControllerBase
{
    private readonly IWarehouseService _service;

    public WarehouseController(IWarehouseService service)
    {
        _service = service;
    }

    /// <summary>
    /// Lista todas las bodegas.
    /// </summary>
    /// <response code="200">Lista de bodegas.</response>
    [HttpGet]
    [ProducesResponseType<Result<IEnumerable<WarehouseDto>>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();

        return StatusCode(result.Status, result);
    }

    /// <summary>
    /// Lista las bodegas por páginas.
    /// </summary>
    /// <param name="pageNumber" example="1">Número de página (desde 1).</param>
    /// <param name="pageSize" example="10">Registros por página (1 a 50).</param>
    /// <response code="200">Página de bodegas con pageNumber, pageSize, totalRecords y totalPages.</response>
    /// <response code="400">pageNumber o pageSize fuera de rango.</response>
    [HttpGet("pagination")]
    [ProducesResponseType<Result<IEnumerable<WarehouseDto>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Result<object>>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPagination([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _service.GetPaginationAsync(pageNumber, pageSize);

        return StatusCode(result.Status, result);
    }

    /// <summary>
    /// Obtiene el detalle de una bodega.
    /// </summary>
    /// <param name="id" example="1">Id de la bodega.</param>
    /// <response code="200">Detalle de la bodega.</response>
    /// <response code="404">La bodega no existe.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType<Result<WarehouseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Result<object>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);

        return StatusCode(result.Status, result);
    }

    /// <summary>
    /// Registra una bodega nueva.
    /// </summary>
    /// <remarks>
    /// El nombre de la bodega es obligatorio y no se puede repetir.
    /// El Id lo asigna el servidor.
    /// </remarks>
    /// <param name="warehouse">Datos de la bodega.</param>
    /// <response code="201">Bodega creada; el encabezado Location apunta a GET api/Warehouse/{id}.</response>
    /// <response code="400">Datos inválidos; data indica el error de cada campo.</response>
    /// <response code="409">Ya existe una bodega con ese nombre.</response>
    [HttpPost]
    [ProducesResponseType<Result<WarehouseDto>>(StatusCodes.Status201Created)]
    [ProducesResponseType<Result<Dictionary<string, string[]>>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<Result<object>>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] WarehouseCreateDto warehouse)
    {
        var result = await _service.AddAsync(warehouse);

        if (!result.IsSuccess)
            return StatusCode(result.Status, result);

        // 201 Created + encabezado Location: /api/Warehouse/{id}
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.IdWarehouse }, result);
    }

    /// <summary>
    /// Actualiza los datos de una bodega.
    /// </summary>
    /// <param name="id" example="1">Id de la bodega (de la ruta, nunca del cuerpo).</param>
    /// <param name="warehouse">Datos nuevos de la bodega.</param>
    /// <response code="200">Bodega actualizada.</response>
    /// <response code="400">Datos inválidos.</response>
    /// <response code="404">La bodega no existe.</response>
    /// <response code="409">El nombre pertenece a otra bodega.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType<Result<WarehouseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Result<Dictionary<string, string[]>>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<Result<object>>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<Result<object>>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] WarehouseUpdateDto warehouse)
    {
        var result = await _service.UpdateAsync(id, warehouse);

        return StatusCode(result.Status, result);
    }

    /// <summary>
    /// Elimina una bodega que no tiene productos asociados.
    /// </summary>
    /// <param name="id" example="1">Id de la bodega.</param>
    /// <response code="200">Bodega eliminada; data = Id eliminado.</response>
    /// <response code="404">La bodega no existe.</response>
    /// <response code="409">La bodega tiene productos y no se puede eliminar.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType<Result<int>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Result<object>>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<Result<object>>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);

        return StatusCode(result.Status, result);
    }
}