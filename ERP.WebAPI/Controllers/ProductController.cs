using ERP.Application.Interfaces;
using ERP.Domain.DTO;
using ERP.Domain.DTO.PatternResult;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
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

    // =====================================================================
    // Lecturas (laboratorios de LINQ) — 6-1: todas responden con Result<T>
    // =====================================================================

    /// <summary>
    /// Lista todos los productos activos.
    /// </summary>
    /// <response code="200">Lista de productos.</response>
    /// <response code="404">No hay productos activos.</response>
    [HttpGet]
    [ProducesResponseType<Result<IEnumerable<ProductListDto>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Result<object>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _service.GetAllAsync();

        return StatusCode(result.Status, result);
    }

    /// <summary>
    /// Lista los productos activos ordenados por nombre de forma descendente.
    /// </summary>
    /// <response code="200">Lista de productos de la Z a la A.</response>
    /// <response code="404">No hay productos activos.</response>
    [HttpGet("SortDesc")]
    [ProducesResponseType<Result<IEnumerable<ProductListDto>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Result<object>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllOrderDescAsync()
    {
        var result = await _service.GetAllOrderDescAsync();

        return StatusCode(result.Status, result);
    }

    /// <summary>
    /// Busca productos cuyo nombre contenga el texto indicado.
    /// </summary>
    /// <param name="name" example="camisa">Texto a buscar en el nombre.</param>
    /// <response code="200">Productos que coinciden.</response>
    /// <response code="404">Ningún producto coincide.</response>
    [HttpGet("{name}/Name")]
    [ProducesResponseType<Result<IEnumerable<ProductListDto>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Result<object>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> FindByNameAsync(string name)
    {
        var result = await _service.FindByNameAsync(name);

        return StatusCode(result.Status, result);
    }

    /// <summary>
    /// Lista los productos activos por páginas.
    /// </summary>
    /// <param name="pageNumber" example="1">Número de página (desde 1).</param>
    /// <param name="pageSize" example="10">Registros por página (1 a 50).</param>
    /// <response code="200">Página de productos con pageNumber, pageSize, totalRecords y totalPages.</response>
    /// <response code="400">pageNumber o pageSize fuera de rango.</response>
    /// <response code="404">La página no tiene productos.</response>
    [HttpGet("{pageNumber}/{pageSize}")]
    [ProducesResponseType<Result<IEnumerable<ProductListDto>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Result<object>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<Result<object>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPaginationAsync(int pageNumber = 1, int pageSize = 10)
    {
        var result = await _service.GetPaginationAsync(pageNumber, pageSize);

        return StatusCode(result.Status, result);
    }

    /// <summary>
    /// Lista los productos con precio mayor o igual al indicado.
    /// </summary>
    /// <param name="price" example="50">Precio mínimo.</param>
    /// <response code="200">Productos ordenados por precio.</response>
    /// <response code="404">Ningún producto alcanza ese precio.</response>
    [HttpGet("{price}/PriceHigher")]
    [ProducesResponseType<Result<IEnumerable<ProductListDto>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Result<object>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPriceAsync(decimal price)
    {
        var result = await _service.GetPriceAsync(price);

        return StatusCode(result.Status, result);
    }

    // GET api/Product/{id:int} → con nombre de ruta (lo usa CreatedAtRoute)
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

    /// <summary>
    /// Cuenta los productos activos.
    /// </summary>
    /// <response code="200">Total de productos activos en data.</response>
    [HttpGet("Count")]
    [ProducesResponseType<Result<int>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> CountAsync()
    {
        var result = await _service.CountAsync();

        return StatusCode(result.Status, result);
    }

    /// <summary>
    /// Lista los 10 productos activos más caros.
    /// </summary>
    /// <response code="200">Los 10 productos de mayor precio.</response>
    /// <response code="404">No hay productos activos.</response>
    [HttpGet("TopPrice")]
    [ProducesResponseType<Result<IEnumerable<ProductListDto>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Result<object>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTop10PriceAsync()
    {
        var result = await _service.GetTop10PriceAsync();

        return StatusCode(result.Status, result);
    }

    /// <summary>
    /// Indica si algún producto activo contiene el texto en su descripción.
    /// </summary>
    /// <param name="description" example="algodón">Texto a buscar en la descripción.</param>
    /// <response code="200">data = true si existe, false si no.</response>
    [HttpGet("{description}/exists")]
    [ProducesResponseType<Result<bool>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> ExistsByDescriptionAsync(string description)
    {
        var result = await _service.ExistsByDescriptionAsync(description);

        return StatusCode(result.Status, result);
    }

    /// <summary>
    /// Lista los productos activos de una categoría.
    /// </summary>
    /// <param name="idCategory" example="1">Id de la categoría.</param>
    /// <response code="200">Productos de la categoría; data = [] si no tiene productos.</response>
    [HttpGet("by-category/{idCategory:int}")]
    [ProducesResponseType<Result<IEnumerable<ProductListDto>>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCategoryAsync(int idCategory)
    {
        var result = await _service.GetByCategoryAsync(idCategory);

        // Categoría sin productos → 200 con [] (no es un error)
        return StatusCode(result.Status, result);
    }

    /// <summary>
    /// Lista los productos cuyo nombre de categoría contenga el texto indicado.
    /// </summary>
    /// <param name="name" example="ropa">Texto a buscar en el nombre de la categoría.</param>
    /// <response code="200">Productos de las categorías que coinciden.</response>
    /// <response code="404">Ningún producto coincide.</response>
    [HttpGet("by-category")]
    [ProducesResponseType<Result<IEnumerable<ProductListDto>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Result<object>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByCategoryNameAsync([FromQuery] string name)
    {
        var result = await _service.GetByCategoryNameAsync(name);

        return StatusCode(result.Status, result);
    }

    /// <summary>
    /// Lista los productos activos de una bodega.
    /// </summary>
    /// <param name="idWarehouse" example="1">Id de la bodega.</param>
    /// <response code="200">Productos de la bodega.</response>
    /// <response code="404">La bodega no tiene productos.</response>
    [HttpGet("by-warehouse/{idWarehouse:int}")]
    [ProducesResponseType<Result<IEnumerable<ProductListDto>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Result<object>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByWarehouseIdAsync(int idWarehouse)
    {
        var result = await _service.GetByWarehouseIdAsync(idWarehouse);

        return StatusCode(result.Status, result);
    }

    /// <summary>
    /// Lista los productos cuyo nombre de bodega contenga el texto indicado.
    /// </summary>
    /// <param name="name" example="sucursal">Texto a buscar en el nombre de la bodega.</param>
    /// <response code="200">Productos de las bodegas que coinciden.</response>
    /// <response code="404">Ningún producto coincide.</response>
    [HttpGet("by-warehouse")]
    [ProducesResponseType<Result<IEnumerable<ProductListDto>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Result<object>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByWarehouseNameAsync([FromQuery] string name)
    {
        var result = await _service.GetByWarehouseNameAsync(name);

        return StatusCode(result.Status, result);
    }

    /// <summary>
    /// Estadísticas por categoría: cantidad, precio mínimo, máximo, promedio y valor del inventario.
    /// </summary>
    /// <response code="200">Una fila de estadísticas por categoría.</response>
    /// <response code="404">No hay productos activos.</response>
    [HttpGet("stats-by-category")]
    [ProducesResponseType<Result<IEnumerable<CategoryStatsDto>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Result<object>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStatsByCategoryAsync()
    {
        var result = await _service.GetStatsByCategoryAsync();

        return StatusCode(result.Status, result);
    }

    // =====================================================================
    // CRUD con relaciones y búsqueda (sin cambios)
    // =====================================================================

    // GET api/Product/search?name=...&minPrice=...&maxPrice=...&idCategory=...
    /// <summary>
    /// Busca productos con filtros, ordenamiento y paginación.
    /// </summary>
    /// <remarks>
    /// Todos los parámetros son opcionales y se combinan con AND.
    /// Ejemplo: GET api/Product/search?idCategory = 1&amp; minPrice=20&amp;maxPrice=60&amp;sortBy=price&amp;sortDirection=desc&amp;pageNumber=1&amp;pageSize=5
    /// </remarks>
    /// <param name="search">Filtros (name, minPrice, maxPrice, idCategory, idSupplier, status), orden(sortBy, sortDirection) y página(pageNumber, pageSize).</param>
    /// <response code="200">Página de resultados; data = [] si no hay coincidencias.</response>
    /// <response code="400">Algún parámetro no es válido (por ejemplo, sortBy=color o pageSize=100).</response>
    [HttpGet("search")]

    [ProducesResponseType<Result<IEnumerable<ProductListDto>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Result<Dictionary<string, string[]>>>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchAsync([FromQuery] ProductSearchDto search)
    {
        var result = await _service.SearchAsync(search);
        return StatusCode(result.Status, result);
    }


    // POST api/Product
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

    // PUT api/Product/{id}
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
    [ProducesResponseType<Result<Dictionary<string, string[]>>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<Result<object>>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<Result<object>>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] ProductUpdateDto product)
    {
        var result = await _service.UpdateAsync(id, product);

        return StatusCode(result.Status, result);
    }

    // DELETE api/Product/{id} (eliminación lógica)
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
}