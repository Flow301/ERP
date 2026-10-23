using ERP.Application.Interfaces;
using ERP.Domain.DTO;
using ERP.Domain.DTO.PatternResult;
using Microsoft.AspNetCore.Mvc;

namespace ERP.WebAPI.Controllers;

/// <summary>
/// Bodegas: consulta, registro, actualización y eliminación.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
[ProducesResponseType<Result<object>>(StatusCodes.Status500InternalServerError)] // aplica a TODAS las acciones
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
    /// <response code="200">Lista de bodegas ordenada por Id.</response>
    [HttpGet]
    [ProducesResponseType<Result<IEnumerable<WarehouseDto>>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync()
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
    [HttpGet("{pageNumber:int}/{pageSize:int}")]
    [ProducesResponseType<Result<IEnumerable<WarehouseDto>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Result<object>>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPaginationAsync(int pageNumber = 1, int pageSize = 10)
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
    [HttpGet("{id:int}", Name = "GetWarehouseById")]
    [ProducesResponseType<Result<WarehouseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Result<object>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var result = await _service.GetByIdAsync(id);

        return StatusCode(result.Status, result);
    }

    /// <summary>
    /// Registra una bodega NUEVA.
    /// </summary>
    /// <remarks>
    /// El nombre es obligatorio (máximo 80 caracteres) y no se puede repetir.
    /// La dirección es opcional (máximo 120 caracteres). El Id lo asigna el servidor.
    /// </remarks>
    /// <param name="warehouse">Datos de la bodega.</param>
    /// <response code="201">Bodega creada; el encabezado Location apunta a GET api/Warehouse/{id}.</response>
    /// <response code="400">Datos inválidos; data indica el error de cada campo.</response>
    /// <response code="409">Ya existe una bodega con ese nombre.</response>
    [HttpPost]
    [ProducesResponseType<Result<WarehouseDto>>(StatusCodes.Status201Created)]
    [ProducesResponseType<Result<Dictionary<string, string[]>>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<Result<object>>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddAsync([FromBody] WarehouseCreateDto warehouse)
    {
        var result = await _service.AddAsync(warehouse);

        if (!result.IsSuccess)
            return StatusCode(result.Status, result);

        // 201 Created + encabezado Location: /api/Warehouse/{id}
        return CreatedAtRoute("GetWarehouseById", new { id = result.Data!.IdWarehouse }, result);
    }

    /// <summary>
    /// Actualiza los datos de una bodega.
    /// </summary>
    /// <remarks>
    /// Se puede conservar el nombre actual; solo es un conflicto si el nombre pertenece a OTRA bodega.
    /// </remarks>
    /// <param name="id" example="1">Id de la bodega (de la ruta, nunca del cuerpo).</param>
    /// <param name="warehouse">Datos NUEVOS de la bodega.</param>
    /// <response code="200">Bodega actualizada.</response>
    /// <response code="400">Datos inválidos; data indica el error de cada campo.</response>
    /// <response code="404">La bodega no existe.</response>
    /// <response code="409">El nombre pertenece a otra bodega.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType<Result<WarehouseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Result<Dictionary<string, string[]>>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<Result<object>>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<Result<object>>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] WarehouseUpdateDto warehouse)
    {
        var result = await _service.UpdateAsync(id, warehouse);

        return StatusCode(result.Status, result);
    }

    /// <summary>
    /// Elimina una bodega que no tiene productos.
    /// </summary>
    /// <param name="id" example="100">Id de la bodega.</param>
    /// <response code="200">Bodega eliminada; data = Id eliminado.</response>
    /// <response code="404">La bodega no existe.</response>
    /// <response code="409">La bodega tiene productos y no se puede eliminar.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType<Result<int>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Result<object>>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<Result<object>>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var result = await _service.DeleteAsync(id);

        return StatusCode(result.Status, result);
    }
}
