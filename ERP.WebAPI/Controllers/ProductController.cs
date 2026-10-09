using ERP.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

        if (!result.Any())
            return NotFound("Not Product found!!!!");

        return Ok(result);
    }

    // GET api/Product/SortDesc
    [HttpGet("SortDesc")]
    public async Task<IActionResult> GetAllOrderDescAsync()
    {
        var result = await _service.GetAllOrderDescAsync();

        if (!result.Any())
            return NotFound("Not Product found!!!!");

        return Ok(result);
    }

    // GET api/Product/{name}/Name
    [HttpGet("{name}/Name")]
    public async Task<IActionResult> FindByNameAsync(string name)
    {
        var result = await _service.FindByNameAsync(name);

        if (!result.Any())
            return NotFound("Not Product found!!!!");

        return Ok(result);
    }

    // GET api/Product/{pageNumber}/{pageSize}
    [HttpGet("{pageNumber}/{pageSize}")]
    public async Task<IActionResult> GetPaginationAsync(int pageNumber = 1, int pageSize = 10)
    {
        var result = await _service.GetPaginationAsync(pageNumber, pageSize);

        if (!result.Any())
            return NotFound("Not Product found!!!!");

        return Ok(result);
    }

    // GET api/Product/{price}/PriceHigher
    [HttpGet("{price}/PriceHigher")]
    public async Task<IActionResult> GetPriceAsync(decimal price)
    {
        var result = await _service.GetPriceAsync(price);

        if (!result.Any())
            return NotFound("Not Product found!!!!");

        return Ok(result);
    }

    // GET api/Product/{id:int}  → ahora devuelve el detalle con relaciones
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var result = await _service.GetByIdAsync(id);

        if (result is null)
            return NotFound("Not Product found!!!!");

        return Ok(result);
    }

    // GET api/Product/Count
    [HttpGet("Count")]
    public async Task<IActionResult> CountAsync()
    {
        var result = await _service.CountAsync();

        return Ok(result);
    }

    // GET api/Product/TopPrice
    [HttpGet("TopPrice")]
    public async Task<IActionResult> GetTop10PriceAsync()
    {
        var result = await _service.GetTop10PriceAsync();

        if (!result.Any())
            return NotFound("Not Product found!!!!");

        return Ok(result);
    }

    // GET api/Product/{description}/exists  (reto A)
    [HttpGet("{description}/exists")]
    public async Task<IActionResult> ExistsByDescriptionAsync(string description)
    {
        var result = await _service.ExistsByDescriptionAsync(description);

        return Ok(result);
    }

    // GET api/Product/by-category/{idCategory:int}  (reto B)
    [HttpGet("by-category/{idCategory:int}")]
    public async Task<IActionResult> GetByCategoryAsync(int idCategory)
    {
        var result = await _service.GetByCategoryAsync(idCategory);

        // Categoría sin productos → 200 con [] (no es un error)
        return Ok(result);
    }

    // =====================================================================
    // NUEVO — LINQ avanzado
    // =====================================================================

    // GET api/Product/by-category?name=ropa
    [HttpGet("by-category")]
    public async Task<IActionResult> GetByCategoryNameAsync([FromQuery] string name)
    {
        var result = await _service.GetByCategoryNameAsync(name);

        if (!result.Any())
            return NotFound("Not Product found!!!!");

        return Ok(result);
    }

    // GET api/Product/by-warehouse/{idWarehouse:int}
    [HttpGet("by-warehouse/{idWarehouse:int}")]
    public async Task<IActionResult> GetByWarehouseIdAsync(int idWarehouse)
    {
        var result = await _service.GetByWarehouseIdAsync(idWarehouse);

        if (!result.Any())
            return NotFound("Not Product found!!!!");

        return Ok(result);
    }

    // GET api/Product/by-warehouse?name=sucursal
    [HttpGet("by-warehouse")]
    public async Task<IActionResult> GetByWarehouseNameAsync([FromQuery] string name)
    {
        var result = await _service.GetByWarehouseNameAsync(name);

        if (!result.Any())
            return NotFound("Not Product found!!!!");

        return Ok(result);
    }

    // GET api/Product/stats-by-category
    [HttpGet("stats-by-category")]
    public async Task<IActionResult> GetStatsByCategoryAsync()
    {
        var result = await _service.GetStatsByCategoryAsync();

        if (!result.Any())
            return NotFound("Not Product found!!!!");

        return Ok(result);
    }
}
