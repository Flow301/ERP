using System.ComponentModel.DataAnnotations;

namespace ERP.Domain.DTO;

/// <summary>
/// Datos para registrar una factura completa: encabezado + líneas, en una sola petición.
/// </summary>
public record class InvoiceCreateDto : IValidatableObject
{
    /// <summary>Cédula del cliente (9 a 12 dígitos, sin guiones).</summary>
    /// <example>101110111</example>
    [Required(ErrorMessage = "El cliente es obligatorio.")]
    [RegularExpression(@"^\d{9,12}$", ErrorMessage = "La cédula del cliente debe tener entre 9 y 12 dígitos, sin guiones.")]
    public string? IdCustomer { get; set; }

    /// <summary>Usuario (vendedor) que registra la venta.</summary>
    /// <example>2</example>
    [Range(1, int.MaxValue, ErrorMessage = "El usuario es obligatorio.")]
    public int IdUser { get; set; }

    /// <summary>Tipo de pago: 1 Efectivo, 2 Tarjeta de Crédito, 3 Tarjeta de Débito, 4 Transferencia, 5 SINPE Móvil, 6 PayPal.</summary>
    /// <example>2</example>
    [Range(1, int.MaxValue, ErrorMessage = "El tipo de pago es obligatorio.")]
    public int IdPaymentType { get; set; }

    /// <summary>Estado inicial: 1 Pendiente o 2 Pagada. Una factura no nace anulada.</summary>
    /// <example>2</example>
    [Range(1, int.MaxValue, ErrorMessage = "El estado de la factura es obligatorio.")]
    public int IdInvoiceStatus { get; set; } = 2; // 2 = Pagada

    /// <summary>Tarjeta (1 Visa, 2 Mastercard...). Solo si el pago es con tarjeta.</summary>
    /// <example>1</example>
    public int? IdCreditCard { get; set; }

    /// <summary>Últimos 4 dígitos de la tarjeta. Solo si el pago es con tarjeta.</summary>
    /// <example>1234</example>
    [RegularExpression(@"^\d{4}$", ErrorMessage = "Solo se guardan los últimos 4 dígitos de la tarjeta.")]
    public string? CreditCardNumber { get; set; }

    /// <summary>Líneas de la factura (1 a 50). Cada producto va en una sola línea.</summary>
    [Required(ErrorMessage = "La factura debe tener líneas.")]
    [Length(1, 50, ErrorMessage = "La factura debe tener entre 1 y 50 líneas.")]
    public List<InvoiceLineCreateDto> Items { get; set; } = new();

    // Reglas entre campos: se ejecutan solo si los atributos de arriba pasaron
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // 2 = Tarjeta de Crédito, 3 = Tarjeta de Débito (IDs estables del CatalogSeeder)
        var requiresCard = IdPaymentType == 2 || IdPaymentType == 3;

        if (requiresCard)
        {
            if (IdCreditCard is null)
                yield return new ValidationResult("La tarjeta es obligatoria cuando el pago es con tarjeta.",
                    [nameof(IdCreditCard)]);

            if (string.IsNullOrWhiteSpace(CreditCardNumber))
                yield return new ValidationResult("Los últimos 4 dígitos son obligatorios cuando el pago es con tarjeta.",
                    [nameof(CreditCardNumber)]);
        }
        else if (IdCreditCard is not null || !string.IsNullOrWhiteSpace(CreditCardNumber))
        {
            yield return new ValidationResult("Solo los pagos con tarjeta llevan datos de tarjeta.",
                [nameof(IdCreditCard), nameof(CreditCardNumber)]);
        }

        var repeated = Items
            .GroupBy(i => i.IdProduct)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (repeated.Any())
            yield return new ValidationResult(
                $"Cada producto va en una sola línea; sume las cantidades. Repetidos: {string.Join(", ", repeated)}.",
                [nameof(Items)]);
    }
}
