namespace ERP.WebAPI;

/// <summary>
/// Nombres de las policies de autorización del ERP (matriz de permisos de la 9-2).
/// </summary>
public static class AuthorizationPolicies
{
    public const string AdminOnly = "AdminOnly";               // Usuarios, Categorías, Bodegas y Proveedores
    public const string ProductManagers = "ProductManagers";   // Productos y consulta de facturas
    public const string SellerOnly = "SellerOnly";             // Registrar facturas
}
