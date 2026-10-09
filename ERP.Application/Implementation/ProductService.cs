using ERP.Application.Interfaces;
using ERP.Domain.DTO;
using ERP.Infraestructure.Interfaces;
using MapsterMapper;

namespace ERP.Application.Implementation;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    // =====================================================================
    // Laboratorio anterior — el repositorio devuelve entidades;
    // el servicio las convierte en DTOs con el mapper
    // =====================================================================

    public async Task<IEnumerable<ProductListDto>> GetAllAsync()
    {
        var products = await _repository.GetAllAsync();

        var result = _mapper.Map<IEnumerable<ProductListDto>>(products);

        return result;
    }

    public async Task<IEnumerable<ProductListDto>> GetAllOrderDescAsync()
    {
        var products = await _repository.GetAllOrderDescAsync();

        var result = _mapper.Map<IEnumerable<ProductListDto>>(products);

        return result;
    }

    public async Task<IEnumerable<ProductListDto>> FindByNameAsync(string name)
    {
        var products = await _repository.FindByNameAsync(name.Trim());

        var result = _mapper.Map<IEnumerable<ProductListDto>>(products);

        return result;
    }

    // Detalle: el mapper arma los DTOs anidados y los campos calculados
    public async Task<ProductDetailDto?> GetByIdAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);

        if (product is null)
            return null;

        var result = _mapper.Map<ProductDetailDto>(product);

        return result;
    }

    public async Task<IEnumerable<ProductListDto>> GetPaginationAsync(int pageNumber, int pageSize)
    {
        var products = await _repository.GetPaginationAsync(pageNumber, pageSize);

        var result = _mapper.Map<IEnumerable<ProductListDto>>(products);

        return result;
    }

    public async Task<IEnumerable<ProductListDto>> GetPriceAsync(decimal price)
    {
        var products = await _repository.GetPriceAsync(price);

        var result = _mapper.Map<IEnumerable<ProductListDto>>(products);

        return result;
    }

    public async Task<IEnumerable<ProductListDto>> GetTop10PriceAsync()
    {
        var products = await _repository.GetTop10PriceAsync();

        var result = _mapper.Map<IEnumerable<ProductListDto>>(products);

        return result;
    }

    // Un número o un bool no necesitan mapper
    public async Task<int> CountAsync()
    {
        var result = await _repository.CountAsync();

        return result;
    }

    public async Task<bool> ExistsByDescriptionAsync(string description)
    {
        var result = await _repository.ExistsByDescriptionAsync(description.Trim());

        return result;
    }

    public async Task<IEnumerable<ProductListDto>> GetByCategoryAsync(int idCategory)
    {
        var products = await _repository.GetByCategoryAsync(idCategory);

        var result = _mapper.Map<IEnumerable<ProductListDto>>(products);

        return result;
    }

    // =====================================================================
    // NUEVO — LINQ avanzado
    // =====================================================================

    public async Task<IEnumerable<ProductListDto>> GetByCategoryNameAsync(string name)
    {
        var products = await _repository.GetByCategoryNameAsync(name.Trim());

        var result = _mapper.Map<IEnumerable<ProductListDto>>(products);

        return result;
    }

    public async Task<IEnumerable<ProductListDto>> GetByWarehouseIdAsync(int idWarehouse)
    {
        var products = await _repository.GetByWarehouseIdAsync(idWarehouse);

        var result = _mapper.Map<IEnumerable<ProductListDto>>(products);

        return result;
    }

    public async Task<IEnumerable<ProductListDto>> GetByWarehouseNameAsync(string name)
    {
        var products = await _repository.GetByWarehouseNameAsync(name.Trim());

        var result = _mapper.Map<IEnumerable<ProductListDto>>(products);

        return result;
    }

    // Excepción: el GroupBy ya llega como DTO desde el repositorio, no necesita mapper
    public async Task<IEnumerable<CategoryStatsDto>> GetStatsByCategoryAsync()
    {
        var result = await _repository.GetStatsByCategoryAsync();

        return result;
    }
}
