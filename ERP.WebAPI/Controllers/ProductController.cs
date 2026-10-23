using ERP.Application.Interfaces;
using ERP.Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using ERP.Domain.DTO;
using ERP.Domain.DTO.PatternResult;

namespace ERP.WebAPI.Controllers;

/// <summary>
/// Productos: catálogo, búsqueda, CRUD con relaciones e imagen.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
[ProducesResponseType<Result<object>>(StatusCodes.Status500InternalServerError)]

public class ProductController : ControllerBase
{
    private readonly IProductService _service;

    public ProductController(IProductService service)
    {
        _service = service;
    }


    // GET api/Product
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _service.GetAllAsync();
        return StatusCode(result.Status, result);
    }
    // GET api/Product/SortDesc
    [HttpGet("SortDesc")]
    public async Task<IActionResult> GetAllOrderDescAsync()
    {
        var result = await _service.GetAllOrderDescAsync();
        return StatusCode(result.Status, result);
    }
    // GET api/Product/{name}/Name
    [HttpGet("{name}/Name")]
    public async Task<IActionResult> FindByNameAsync(string name)
    {
        var result = await _service.FindByNameAsync(name);
        return StatusCode(result.Status, result);
    }
    // GET api/Product/{pageNumber}/{pageSize}
    [HttpGet("{pageNumber}/{pageSize}")]
    public async Task<IActionResult> GetPaginationAsync(int pageNumber = 1, int pageSize =
   10)
    {
        var result = await _service.GetPaginationAsync(pageNumber, pageSize);
        return StatusCode(result.Status, result);
    }
    // GET api/Product/{price}/PriceHigher
    [HttpGet("{price}/PriceHigher")]
    public async Task<IActionResult> GetPriceAsync(decimal price)
    {
        var result = await _service.GetPriceAsync(price);
        return StatusCode(result.Status, result);
    }
    /// <summary>
    /// Obtiene el detalle de un producto con su categoría, bodega y proveedores.
    /// </summary>
    /// <param name="id" example="1">Id del producto.</param>
    /// <response code="200">Detalle del producto.</response>
    /// <response code="404">El producto no existe o está inactivo.</response>
    [HttpGet("{id:int}", Name = "GetProductById")]
    [ProducesResponseType<Result<ProductDetailDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Result<object>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(int id)

    {
        var result = await _service.GetByIdAsync(id);
        return StatusCode(result.Status, result);
    }
    // GET api/Product/Count
    [HttpGet("Count")]
    public async Task<IActionResult> CountAsync()
    {
        var result = await _service.CountAsync();
        return StatusCode(result.Status, result);
    }
    // GET api/Product/TopPrice
    [HttpGet("TopPrice")]
    public async Task<IActionResult> GetTop10PriceAsync()
    {
        var result = await _service.GetTop10PriceAsync();
        return StatusCode(result.Status, result);
    }
    // GET api/Product/{description}/exists
    [HttpGet("{description}/exists")]
    public async Task<IActionResult> ExistsByDescriptionAsync(string description)
    {
        var result = await _service.ExistsByDescriptionAsync(description);
        return StatusCode(result.Status, result);
    }
    // GET api/Product/by-category/{idCategory:int}
    [HttpGet("by-category/{idCategory:int}")]
    public async Task<IActionResult> GetByCategoryAsync(int idCategory)
    {
        var result = await _service.GetByCategoryAsync(idCategory);
        // Categoría sin productos → 200 con [] (no es un error)
        return StatusCode(result.Status, result);
    }
    // GET api/Product/by-category?name=ropa
    [HttpGet("by-category")]
    public async Task<IActionResult> GetByCategoryNameAsync([FromQuery] string name)
    {
        var result = await _service.GetByCategoryNameAsync(name);
        return StatusCode(result.Status, result);
            }
    // GET api/Product/by-warehouse/{idWarehouse:int}
    [HttpGet("by-warehouse/{idWarehouse:int}")]
    public async Task<IActionResult> GetByWarehouseIdAsync(int idWarehouse)
    {
        var result = await _service.GetByWarehouseIdAsync(idWarehouse);
        return StatusCode(result.Status, result);
    }
    // GET api/Product/by-warehouse?name=sucursal
    [HttpGet("by-warehouse")]
    public async Task<IActionResult> GetByWarehouseNameAsync([FromQuery] string name)
    {
        var result = await _service.GetByWarehouseNameAsync(name);
        return StatusCode(result.Status, result);
    }
    // GET api/Product/stats-by-category
    [HttpGet("stats-by-category")]
    public async Task<IActionResult> GetStatsByCategoryAsync()
    {
        var result = await _service.GetStatsByCategoryAsync();
        return StatusCode(result.Status, result);
    }

    // =====================================================================
    // NUEVO — CRUD con relaciones (respuestas con Result<T>)
    // =====================================================================
    /// <summary>
    /// Registra un producto con sus proveedores.
    /// </summary>
    /// <remarks>
    /// La categoría, la bodega y los proveedores deben existir. El nombre no se puede repetir.
    /// La imagen se sube después con POST api/Product/{id}/image.
    /// </remarks>
    /// <param name="product">Datos del producto y lista de Ids de proveedores.</param>
    /// <response code="201">Producto creado; Location apunta a GET api/Product/{id}.</response>
 /// <response code="400">Datos inválidos, o una categoría, bodega o proveedor no existe.</response>
 /// <response code="409">Ya existe un producto con ese nombre.</response>
 [HttpPost]
[ProducesResponseType<Result<ProductDetailDto>>(StatusCodes.Status201Created)]
[ProducesResponseType<Result<Dictionary<string,
string[]>>>(StatusCodes.Status400BadRequest)]
[ProducesResponseType<Result<object>>(StatusCodes.Status409Conflict)]
public async Task<IActionResult> AddAsync([FromBody] ProductCreateDto product)
{
        var result = await _service.AddAsync(product);
        if (!result.IsSuccess)
            return StatusCode(result.Status, result);
        // 201 Created + encabezado Location: /api/Product/{id}
        return CreatedAtRoute("GetProductById", new { id = result.Data!.IdProduct }, result);
    }
    /// <summary>
    /// Actualiza un producto y sincroniza sus proveedores.
    /// </summary>
    /// <param name="id" example="1">Id del producto.</param>
    /// <param name="product">Datos NUEVOs. La imagen no se cambia aquí.</param>
    /// <response code="200">Producto actualizado.</response>
    /// <response code="400">Datos inválidos, o una relación no existe.</response>
    /// <response code="404">El producto no existe o está inactivo.</response>
    /// <response code="409">Otro producto ya tiene ese nombre.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType<Result<ProductDetailDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Result<Dictionary<string,
   string[]>>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<Result<object>>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<Result<object>>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody]
ProductUpdateDto product)

    {
        var result = await _service.UpdateAsync(id, product);
        return StatusCode(result.Status, result);
    }
    /// <summary>
    /// Desactiva un producto (eliminación lógica: Status = false).
    /// </summary>
    /// <param name="id" example="1">Id del producto.</param>
    /// <response code="200">Producto desactivado; data = Id.</response>
    /// <response code="404">El producto no existe o ya estaba inactivo.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType<Result<int>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Result<object>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var result = await _service.DeleteAsync(id);
        return StatusCode(result.Status, result);
    }
    // GET api/Product/search?name=...&minPrice=...&maxPrice=...&idCategory=...
    [HttpGet("search")]
    public async Task<IActionResult> SearchAsync([FromQuery] ProductSearchDto search)
    {
        var result = await _service.SearchAsync(search);
        return StatusCode(result.Status, result);
    }

}
