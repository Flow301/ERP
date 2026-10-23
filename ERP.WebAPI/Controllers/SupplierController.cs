using ERP.Application.Interfaces;
using ERP.Domain.DTO;
using ERP.Domain.DTO.PatternResult;
using Microsoft.AspNetCore.Mvc;
namespace ERP.WebAPI.Controllers;
/// <summary>
/// Proveedores: consulta, registro, actualización y eliminación.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
[ProducesResponseType<Result<object>>(StatusCodes.Status500InternalServerError)] //aplica a TODAS las acciones
    public class SupplierController : ControllerBase
{
    private readonly ISupplierService _service;
    public SupplierController(ISupplierService service)
    {
        _service = service;
    }
    /// <summary>
    /// Lista todos los proveedores.
    /// </summary>
    /// <response code="200">Lista de proveedores.</response>
    [HttpGet]

    [ProducesResponseType<Result<IEnumerable<SupplierListDto>>>(StatusCodes.Status200OK)
   ]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _service.GetAllAsync();
        return StatusCode(result.Status, result);
    }
    /// <summary>
    /// Lista los proveedores por páginas.
    /// </summary>
    /// <param name="pageNumber" example="1">Número de página (desde 1).</param>
    /// <param name="pageSize" example="10">Registros por página (1 a 50).</param>
    /// <response code="200">Página de proveedores con pageNumber, pageSize, totalRecords y totalPages.</response>
        /// <response code="400">pageNumber o pageSize fuera de rango.</response>
 [HttpGet("{pageNumber:int}/{pageSize:int}")]

    [ProducesResponseType<Result<IEnumerable<SupplierListDto>>>(StatusCodes.Status200OK)
]
    [ProducesResponseType<Result<object>>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPaginationAsync(int pageNumber = 1, int pageSize =
10)
    {
        var result = await _service.GetPaginationAsync(pageNumber, pageSize);
        return StatusCode(result.Status, result);
    }
    /// <summary>
    /// Obtiene el detalle de un proveedor.
    /// </summary>
    /// <param name="id" example="1">Id del proveedor.</param>
    /// <response code="200">Detalle del proveedor.</response>
    /// <response code="404">El proveedor no existe.</response>
    [HttpGet("{id:int}", Name = "GetSupplierById")]
    [ProducesResponseType<Result<SupplierDetailDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Result<object>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return StatusCode(result.Status, result);
    }
    /// <summary>
    /// /// Registra un proveedor NUEVO.
    /// </summary>
    /// <remarks>
    /// La cédula jurídica tiene el formato 3-101-123456 y no se puede repetir.
    /// El proveedor nace activo; el Id lo asigna el servidor.
    /// </remarks>
    /// <param name="supplier">Datos del proveedor.</param>
    /// <response code="201">Proveedor creado; el encabezado Location apunta a GET api/Supplier/{id}.</response>
 /// <response code="400">Datos inválidos; data indica el error de cada campo.</response>
 /// <response code="409">Ya existe un proveedor con esa cédula jurídica.</response>
 [HttpPost]
[ProducesResponseType<Result<SupplierDetailDto>>(StatusCodes.Status201Created)]
[ProducesResponseType<Result<Dictionary<string,
string[]>>>(StatusCodes.Status400BadRequest)]
[ProducesResponseType<Result<object>>(StatusCodes.Status409Conflict)]
public async Task<IActionResult> AddAsync([FromBody] SupplierCreateDto supplier)
{
    var result = await _service.AddAsync(supplier);
    if (!result.IsSuccess)
        return StatusCode(result.Status, result);
    // 201 Created + encabezado Location: /api/Supplier/{id}
    return CreatedAtRoute("GetSupplierById", new { id = result.Data!.IdSupplier }, result);
}
/// <summary>
/// Actualiza los datos de un proveedor.
/// </summary>
/// <param name="id" example="1">Id del proveedor (de la ruta, nunca del cuerpo).</param>
 /// <param name="supplier">Datos NUEVOs del proveedor.</param>
 /// <response code="200">Proveedor actualizado.</response>
 /// <response code="400">Datos inválidos.</response>
 /// <response code="404">El proveedor no existe.</response>
 /// <response code="409">La cédula jurídica pertenece a otro proveedor.</response>
 [HttpPut("{id:int}")]
[ProducesResponseType<Result<SupplierDetailDto>>(StatusCodes.Status200OK)]
[ProducesResponseType<Result<Dictionary<string,
string[]>>>(StatusCodes.Status400BadRequest)]
[ProducesResponseType<Result<object>>(StatusCodes.Status404NotFound)]
[ProducesResponseType<Result<object>>(StatusCodes.Status409Conflict)]
public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody]
SupplierUpdateDto supplier)
{
    var result = await _service.UpdateAsync(id, supplier);
    return StatusCode(result.Status, result);
}
/// <summary>
/// Elimina un proveedor que no tiene productos asociados.
/// </summary>
/// <param name="id" example="101">Id del proveedor.</param>
/// <response code="200">Proveedor eliminado; data = Id eliminado.</response>
/// <response code="404">El proveedor no existe.</response>
/// <response code="409">El proveedor tiene productos y no se puede eliminar.</response>
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
