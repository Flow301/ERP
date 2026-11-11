using System.ComponentModel.DataAnnotations;

namespace ERP.Domain.DTO;

/// <summary>
/// Una línea de la factura. El precio NO viene del cliente: se toma del catálogo al guardar.
/// </summary>
public record class InvoiceLineCreateDto
{
    /// <summary>Producto vendido.</summary>
    /// <example>1</example>
    [Range(1, int.MaxValue, ErrorMessage = "El producto es obligatorio.")]
    public int IdProduct { get; set; }

    /// <summary>Cantidad vendida (1 a 1000).</summary>
    /// <example>2</example>
    [Range(1, 1000, ErrorMessage = "La cantidad debe estar entre 1 y 1000.")]
    public int Quantity { get; set; }

    /// <summary>Impuesto de la línea: 1 Impuesto de Ventas (13 %), 2 Impuesto Selectivo (15 %).</summary>
    /// <example>1</example>
    [Range(1, int.MaxValue, ErrorMessage = "El impuesto no es válido.")]
    public int IdTax { get; set; } = 1; // 1 = Impuesto de Ventas (IVA 13 %)
}
