using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ERP.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Category",
                schema: "dbo",
                columns: table => new
                {
                    IdCategory = table.Column<int>(type: "int", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriaRopa", x => x.IdCategory);
                });

            migrationBuilder.CreateTable(
                name: "CreditCard",
                columns: table => new
                {
                    IdCreditCard = table.Column<int>(type: "int", nullable: false),
                    CreditCardName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarjeta", x => x.IdCreditCard);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceStatuse",
                columns: table => new
                {
                    IdInvoiceStatus = table.Column<int>(type: "int", nullable: false),
                    InvoiceStatusDescription = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoFactura", x => x.IdInvoiceStatus);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTypes",
                columns: table => new
                {
                    IdPaymentType = table.Column<int>(type: "int", nullable: false),
                    PaymentTypeDescription = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoPago", x => x.IdPaymentType);
                });

            migrationBuilder.CreateTable(
                name: "Province",
                columns: table => new
                {
                    IdProvince = table.Column<int>(type: "int", nullable: false),
                    ProvinceName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provincia", x => x.IdProvince);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    IdRole = table.Column<int>(type: "int", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol", x => x.IdRole);
                });

            migrationBuilder.CreateTable(
                name: "Supplier",
                columns: table => new
                {
                    IdSupplier = table.Column<int>(type: "int", nullable: false),
                    SupplierName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LegalEntityNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedor", x => x.IdSupplier);
                });

            migrationBuilder.CreateTable(
                name: "Taxe",
                columns: table => new
                {
                    IdTax = table.Column<int>(type: "int", nullable: false),
                    TaxName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Percentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Impuesto", x => x.IdTax);
                });

            migrationBuilder.CreateTable(
                name: "Warehouse",
                columns: table => new
                {
                    IdWarehouse = table.Column<int>(type: "int", nullable: false),
                    WarehouseName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bodega", x => x.IdWarehouse);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    IdCustomer = table.Column<string>(type: "nchar(15)", fixedLength: true, maxLength: 15, nullable: false),
                    IdProvince = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.IdCustomer);
                    table.ForeignKey(
                        name: "FK_Cliente_Provincia",
                        column: x => x.IdProvince,
                        principalTable: "Province",
                        principalColumn: "IdProvince");
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    IdUser = table.Column<int>(type: "int", nullable: false),
                    IdRole = table.Column<int>(type: "int", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.IdUser);
                    table.ForeignKey(
                        name: "FK_Usuario_Rol",
                        column: x => x.IdRole,
                        principalTable: "Role",
                        principalColumn: "IdRole");
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    IdProduct = table.Column<int>(type: "int", nullable: false),
                    IdWarehouse = table.Column<int>(type: "int", nullable: false),
                    IdCategory = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysdatetime())"),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ropa", x => x.IdProduct);
                    table.ForeignKey(
                        name: "FK_Ropa_Bodega",
                        column: x => x.IdWarehouse,
                        principalTable: "Warehouse",
                        principalColumn: "IdWarehouse");
                    table.ForeignKey(
                        name: "FK_Ropa_CategoriaRopa",
                        column: x => x.IdCategory,
                        principalSchema: "dbo",
                        principalTable: "Category",
                        principalColumn: "IdCategory");
                });

            migrationBuilder.CreateTable(
                name: "Invoice",
                columns: table => new
                {
                    IdInvoice = table.Column<int>(type: "int", nullable: false),
                    IdCustomer = table.Column<string>(type: "nchar(15)", fixedLength: true, maxLength: 15, nullable: false),
                    IdUser = table.Column<int>(type: "int", nullable: false),
                    IdInvoiceStatus = table.Column<int>(type: "int", nullable: false),
                    IdPaymentType = table.Column<int>(type: "int", nullable: false),
                    IdCreditCard = table.Column<int>(type: "int", nullable: true),
                    CreditCardNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacturaEncabezado", x => x.IdInvoice);
                    table.ForeignKey(
                        name: "FK_FacturaEncabezado_Cliente",
                        column: x => x.IdCustomer,
                        principalTable: "Customer",
                        principalColumn: "IdCustomer");
                    table.ForeignKey(
                        name: "FK_FacturaEncabezado_EstadoFactura",
                        column: x => x.IdInvoiceStatus,
                        principalTable: "InvoiceStatuse",
                        principalColumn: "IdInvoiceStatus");
                    table.ForeignKey(
                        name: "FK_FacturaEncabezado_Tarjeta",
                        column: x => x.IdCreditCard,
                        principalTable: "CreditCard",
                        principalColumn: "IdCreditCard");
                    table.ForeignKey(
                        name: "FK_FacturaEncabezado_TipoPago",
                        column: x => x.IdPaymentType,
                        principalTable: "PaymentTypes",
                        principalColumn: "IdPaymentType");
                    table.ForeignKey(
                        name: "FK_FacturaEncabezado_Usuario",
                        column: x => x.IdUser,
                        principalTable: "User",
                        principalColumn: "IdUser");
                });

            migrationBuilder.CreateTable(
                name: "ProductSupplier",
                columns: table => new
                {
                    IdProduct = table.Column<int>(type: "int", nullable: false),
                    IdSupplier = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RopaProveedor", x => new { x.IdProduct, x.IdSupplier });
                    table.ForeignKey(
                        name: "FK_RopaProveedor_Proveedor",
                        column: x => x.IdSupplier,
                        principalTable: "Supplier",
                        principalColumn: "IdSupplier");
                    table.ForeignKey(
                        name: "FK_RopaProveedor_Ropa",
                        column: x => x.IdProduct,
                        principalTable: "Product",
                        principalColumn: "IdProduct");
                });

            migrationBuilder.CreateTable(
                name: "InvoiceItem",
                columns: table => new
                {
                    IdInvoice = table.Column<int>(type: "int", nullable: false),
                    LineNumber = table.Column<int>(type: "int", nullable: false),
                    IdProduct = table.Column<int>(type: "int", nullable: false),
                    IdTax = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceItems", x => new { x.IdInvoice, x.LineNumber });
                    table.ForeignKey(
                        name: "FK_FacturaDetalle_Impuesto",
                        column: x => x.IdTax,
                        principalTable: "Taxe",
                        principalColumn: "IdTax");
                    table.ForeignKey(
                        name: "FK_FacturaDetalle_Ropa",
                        column: x => x.IdProduct,
                        principalTable: "Product",
                        principalColumn: "IdProduct");
                    table.ForeignKey(
                        name: "FK_InvoiceItems_Invoice",
                        column: x => x.IdInvoice,
                        principalTable: "Invoice",
                        principalColumn: "IdInvoice");
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Category",
                columns: new[] { "IdCategory", "CategoryName" },
                values: new object[,]
                {
                    { 1, "Ropa" },
                    { 2, "Zapatos" },
                    { 3, "Bolsos" },
                    { 4, "Accesorios" },
                    { 5, "Ropa Deportiva" },
                    { 6, "Ropa Interior" },
                    { 7, "Joyería" },
                    { 8, "Cinturones" },
                    { 9, "Gorras y Sombreros" },
                    { 10, "Otros" }
                });

            migrationBuilder.InsertData(
                table: "CreditCard",
                columns: new[] { "IdCreditCard", "CreditCardName" },
                values: new object[,]
                {
                    { 1, "Visa" },
                    { 2, "Mastercard" },
                    { 3, "American Express" },
                    { 4, "Discover" },
                    { 5, "Diners Club" },
                    { 6, "JCB" }
                });

            migrationBuilder.InsertData(
                table: "InvoiceStatuse",
                columns: new[] { "IdInvoiceStatus", "InvoiceStatusDescription" },
                values: new object[,]
                {
                    { 1, "Pendiente" },
                    { 2, "Pagada" },
                    { 3, "Anulada" }
                });

            migrationBuilder.InsertData(
                table: "PaymentTypes",
                columns: new[] { "IdPaymentType", "PaymentTypeDescription" },
                values: new object[,]
                {
                    { 1, "Efectivo" },
                    { 2, "Tarjeta de Crédito" },
                    { 3, "Tarjeta de Débito" },
                    { 4, "Transferencia Bancaria" },
                    { 5, "SINPE Móvil" },
                    { 6, "PayPal" }
                });

            migrationBuilder.InsertData(
                table: "Province",
                columns: new[] { "IdProvince", "ProvinceName" },
                values: new object[,]
                {
                    { 1, "San José" },
                    { 2, "Alajuela" },
                    { 3, "Cartago" },
                    { 4, "Heredia" },
                    { 5, "Guanacaste" },
                    { 6, "Puntarenas" },
                    { 7, "Limón" }
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "IdRole", "RoleName" },
                values: new object[,]
                {
                    { 1, "Administrador" },
                    { 2, "Vendedor" },
                    { 3, "Cliente" }
                });

            migrationBuilder.InsertData(
                table: "Taxe",
                columns: new[] { "IdTax", "Percentage", "TaxName" },
                values: new object[,]
                {
                    { 1, 13m, "Impuesto de Ventas" },
                    { 2, 15m, "Impuesto Selectivo" }
                });

            migrationBuilder.InsertData(
                table: "Warehouse",
                columns: new[] { "IdWarehouse", "Address", "WarehouseName" },
                values: new object[,]
                {
                    { 1, "San José Centro", "Bodega Central" },
                    { 2, "Escazú, San José", "Sucursal Escazú" },
                    { 3, "Heredia Centro", "Sucursal Heredia" },
                    { 4, "Alajuela Centro", "Sucursal Alajuela" },
                    { 5, "Cartago Centro", "Sucursal Cartago" },
                    { 6, "San Carlos, Alajuela", "Centro de Distribución Norte" },
                    { 7, "Pérez Zeledón, San José", "Centro de Distribución Sur" }
                });

            migrationBuilder.CreateIndex(
                name: "UQ_CategoriaRopa_DescripcionCategoria",
                schema: "dbo",
                table: "Category",
                column: "CategoryName");

            migrationBuilder.CreateIndex(
                name: "UQ_Tarjeta_DescripcionTarjeta",
                table: "CreditCard",
                column: "CreditCardName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_IdProvince",
                table: "Customer",
                column: "IdProvince");

            migrationBuilder.CreateIndex(
                name: "UQ_Cliente_Email",
                table: "Customer",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_IdCreditCard",
                table: "Invoice",
                column: "IdCreditCard");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_IdCustomer",
                table: "Invoice",
                column: "IdCustomer");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_IdInvoiceStatus",
                table: "Invoice",
                column: "IdInvoiceStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_IdPaymentType",
                table: "Invoice",
                column: "IdPaymentType");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_IdUser",
                table: "Invoice",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItem_IdProduct",
                table: "InvoiceItem",
                column: "IdProduct");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItem_IdTax",
                table: "InvoiceItem",
                column: "IdTax");

            migrationBuilder.CreateIndex(
                name: "UQ_EstadoFactura_DescripcionEstado",
                table: "InvoiceStatuse",
                column: "InvoiceStatusDescription",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_TipoPago_DescripcionTipoPago",
                table: "PaymentTypes",
                column: "PaymentTypeDescription",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_IdCategory",
                table: "Product",
                column: "IdCategory");

            migrationBuilder.CreateIndex(
                name: "IX_Product_IdWarehouse",
                table: "Product",
                column: "IdWarehouse");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSupplier_IdSupplier",
                table: "ProductSupplier",
                column: "IdSupplier");

            migrationBuilder.CreateIndex(
                name: "UQ_Provincia_Descripcion",
                table: "Province",
                column: "ProvinceName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Rol_DescripcionRol",
                table: "Role",
                column: "RoleName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Proveedor_CedulaJuridica",
                table: "Supplier",
                column: "LegalEntityNumber",
                unique: true,
                filter: "[LegalEntityNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ_Proveedor_Email",
                table: "Supplier",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ_Impuesto_DescripcionImpuesto",
                table: "Taxe",
                column: "TaxName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_IdRole",
                table: "User",
                column: "IdRole");

            migrationBuilder.CreateIndex(
                name: "UQ_Usuario_Email",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Usuario_Login",
                table: "User",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Bodega_DescripcionBodega",
                table: "Warehouse",
                column: "WarehouseName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceItem");

            migrationBuilder.DropTable(
                name: "ProductSupplier");

            migrationBuilder.DropTable(
                name: "Taxe");

            migrationBuilder.DropTable(
                name: "Invoice");

            migrationBuilder.DropTable(
                name: "Supplier");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "InvoiceStatuse");

            migrationBuilder.DropTable(
                name: "CreditCard");

            migrationBuilder.DropTable(
                name: "PaymentTypes");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Warehouse");

            migrationBuilder.DropTable(
                name: "Category",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Province");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
