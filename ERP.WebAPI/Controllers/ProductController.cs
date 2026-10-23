using ERP.Application.Interfaces;
using ERP.Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using ERP.Domain.DTO;

namespace ERP.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
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
    // GET api/Product/{id:int} → con nombre de ruta (lo usa CreatedAtRoute)
    [HttpGet("{id:int}", Name = "GetProductById")]
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
    // POST api/Product
    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] ProductCreateDto product)
    {
        var result = await _service.AddAsync(product);
        if (!result.IsSuccess)
            return StatusCode(result.Status, result);
        // 201 Created + encabezado Location: /api/Product/{id}
        return CreatedAtRoute("GetProductById", new { id = result.Data!.IdProduct }, result);
    }
    // PUT api/Product/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody]
ProductUpdateDto product)
    {
        var result = await _service.UpdateAsync(id, product);
        return StatusCode(result.Status, result);
    }
    // DELETE api/Product/{id} (eliminación lógica)
    [HttpDelete("{id:int}")]
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
