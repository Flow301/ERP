using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace ERP.Infraestructure.Data.Seed
{
	/// <summary>
	/// SEED MAESTRO DE CATÁLOGOS (HasData).
	/// Datos pequeños, estables y con IDs fijos. Forman parte del modelo:
	/// viajan dentro de las migraciones y llegan a TODOS los ambientes.
	/// </summary>
	public static class CatalogSeeder
	{
		public static void SeedCatalogs(this ModelBuilder modelBuilder)
		{
			// Category
			modelBuilder.Entity<Category>().HasData(
			new Category { IdCategory = 1, CategoryName = "Ropa" },
			new Category { IdCategory = 2, CategoryName = "Zapatos" },
			new Category { IdCategory = 3, CategoryName = "Bolsos" },
			new Category { IdCategory = 4, CategoryName = "Accesorios" },
			new Category { IdCategory = 5, CategoryName = "Ropa Deportiva" },
			new Category { IdCategory = 6, CategoryName = "Ropa Interior" },
			new Category { IdCategory = 7, CategoryName = "Joyería" },
			new Category { IdCategory = 8, CategoryName = "Cinturones" },
			new Category { IdCategory = 9, CategoryName = "Gorras y Sombreros" },
			new Category { IdCategory = 10, CategoryName = "Otros" }
			);
			// Los siguientes catálogos se agregan aquí en las secciones 3 a 6

			// CreditCard
			modelBuilder.Entity<CreditCard>().HasData(
			 new CreditCard
			 {
				 IdCreditCard = 1,
				 CreditCardName = "Visa"
			 },
			 new CreditCard
			 {
				 IdCreditCard = 2,
				 CreditCardName = "Mastercard"
			 },
			 new CreditCard
			 {
				 IdCreditCard = 3,
				 CreditCardName = "American Express"
			 },
			 new CreditCard
			 {
				 IdCreditCard = 4,
				 CreditCardName = "Discover"
			 },
			 new CreditCard
			 {
				 IdCreditCard = 5,
				 CreditCardName = "Diners Club"
			 },
			new CreditCard
			{
				 IdCreditCard = 6,
				 CreditCardName = "JCB"
			}
			);

            // WareHouse
            modelBuilder.Entity<Warehouse>().HasData(
             new Warehouse
             {
                 IdWarehouse = 1,
                 WarehouseName = "Bodega Central",
                 Address = "San José Centro"
             },
             new Warehouse
             {
                 IdWarehouse = 2,
                 WarehouseName = "Sucursal Escazú",
                 Address = "Escazú, San José"
             },
             new Warehouse
             {
                 IdWarehouse = 3,
                 WarehouseName = "Sucursal Heredia",
                 Address = "Heredia Centro"
             },
             new Warehouse
             {
                 IdWarehouse = 4,
                 WarehouseName = "Sucursal Alajuela",
                 Address = "Alajuela Centro"
             },
			 new Warehouse
			 {
				 IdWarehouse = 5,
				 WarehouseName = "Sucursal Cartago",
				 Address = "Cartago Centro"
			 },
			 new Warehouse
			 {
				 IdWarehouse = 6,
				 WarehouseName = "Centro de Distribución Norte",
				 Address = "San Carlos, Alajuela"
			 },
			 new Warehouse
			 {
				 IdWarehouse = 7,
				 WarehouseName = "Centro de Distribución Sur",
				 Address = "Pérez Zeledón, San José"
			 }
			 );

            // Bodegas adicionales para practicar paginación (reto del laboratorio CRUD)
            modelBuilder.Entity<Warehouse>().HasData(
                new Warehouse { IdWarehouse = 11, WarehouseName = "Bodega Central San José", Address = "San José, San José" },
                new Warehouse { IdWarehouse = 12, WarehouseName = "Bodega Metropolitana Escazú", Address = "San José, Escazú" },
                new Warehouse { IdWarehouse = 13, WarehouseName = "Bodega Logística Desamparados", Address = "San José, Desamparados" },
                new Warehouse { IdWarehouse = 14, WarehouseName = "Bodega Regional Alajuela", Address = "Alajuela, Alajuela" },
                new Warehouse { IdWarehouse = 15, WarehouseName = "Bodega Occidente San Ramón", Address = "Alajuela, San Ramón" },
                new Warehouse { IdWarehouse = 16, WarehouseName = "Bodega Industrial Cartago", Address = "Cartago, Cartago" },
                new Warehouse { IdWarehouse = 17, WarehouseName = "Bodega Valle de Paraíso", Address = "Cartago, Paraíso" },
                new Warehouse { IdWarehouse = 18, WarehouseName = "Bodega Heredia Norte", Address = "Heredia, Heredia" },
                new Warehouse { IdWarehouse = 19, WarehouseName = "Bodega Guanacaste Liberia", Address = "Guanacaste, Liberia" },
                new Warehouse { IdWarehouse = 20, WarehouseName = "Bodega Pacífico Puntarenas", Address = "Puntarenas, Puntarenas" },
                new Warehouse { IdWarehouse = 21, WarehouseName = "Bodega Central Tibás", Address = "San José, Tibás" },
                new Warehouse { IdWarehouse = 22, WarehouseName = "Bodega Logística Moravia", Address = "San José, Moravia" },
                new Warehouse { IdWarehouse = 23, WarehouseName = "Bodega Metropolitana Goicoechea", Address = "San José, Goicoechea" },
                new Warehouse { IdWarehouse = 24, WarehouseName = "Bodega Este Curridabat", Address = "San José, Curridabat" },
                new Warehouse { IdWarehouse = 25, WarehouseName = "Bodega Sur Aserrí", Address = "San José, Aserrí" },
                new Warehouse { IdWarehouse = 26, WarehouseName = "Bodega Valle Puriscal", Address = "San José, Puriscal" },
                new Warehouse { IdWarehouse = 27, WarehouseName = "Bodega Industrial Turrubares", Address = "San José, Turrubares" },
                new Warehouse { IdWarehouse = 28, WarehouseName = "Bodega Regional Dota", Address = "San José, Dota" },
                new Warehouse { IdWarehouse = 29, WarehouseName = "Bodega Montaña Tarrazú", Address = "San José, Tarrazú" },
                new Warehouse { IdWarehouse = 30, WarehouseName = "Bodega Los Santos León Cortés", Address = "San José, León Cortés" },
                new Warehouse { IdWarehouse = 31, WarehouseName = "Bodega Norte Grecia", Address = "Alajuela, Grecia" },
                new Warehouse { IdWarehouse = 32, WarehouseName = "Bodega Occidente Naranjo", Address = "Alajuela, Naranjo" },
                new Warehouse { IdWarehouse = 33, WarehouseName = "Bodega Cafetal Palmares", Address = "Alajuela, Palmares" },
                new Warehouse { IdWarehouse = 34, WarehouseName = "Bodega Industrial Poás", Address = "Alajuela, Poás" },
                new Warehouse { IdWarehouse = 35, WarehouseName = "Bodega Volcán Atenas", Address = "Alajuela, Atenas" },
                new Warehouse { IdWarehouse = 36, WarehouseName = "Bodega Regional Orotina", Address = "Alajuela, Orotina" },
                new Warehouse { IdWarehouse = 37, WarehouseName = "Bodega Frontera San Carlos", Address = "Alajuela, San Carlos" },
                new Warehouse { IdWarehouse = 38, WarehouseName = "Bodega Pital Guatuso", Address = "Alajuela, Guatuso" },
                new Warehouse { IdWarehouse = 39, WarehouseName = "Bodega Norte Upala", Address = "Alajuela, Upala" },
                new Warehouse { IdWarehouse = 40, WarehouseName = "Bodega Llanuras Los Chiles", Address = "Alajuela, Los Chiles" },
                new Warehouse { IdWarehouse = 41, WarehouseName = "Bodega Cartago Centro", Address = "Cartago, Cartago" },
                new Warehouse { IdWarehouse = 42, WarehouseName = "Bodega Valle Oreamuno", Address = "Cartago, Oreamuno" },
                new Warehouse { IdWarehouse = 43, WarehouseName = "Bodega Colonial El Guarco", Address = "Cartago, El Guarco" },
                new Warehouse { IdWarehouse = 44, WarehouseName = "Bodega Norte Alvarado", Address = "Cartago, Alvarado" },
                new Warehouse { IdWarehouse = 45, WarehouseName = "Bodega Este Jiménez", Address = "Cartago, Jiménez" },
                new Warehouse { IdWarehouse = 46, WarehouseName = "Bodega Agrícola Turrialba", Address = "Cartago, Turrialba" },
                new Warehouse { IdWarehouse = 47, WarehouseName = "Bodega Valle Central Paraíso", Address = "Cartago, Paraíso" },
                new Warehouse { IdWarehouse = 48, WarehouseName = "Bodega Montaña Cervantes", Address = "Cartago, Alvarado" },
                new Warehouse { IdWarehouse = 49, WarehouseName = "Bodega Industrial Tucurrique", Address = "Cartago, Jiménez" },
                new Warehouse { IdWarehouse = 50, WarehouseName = "Bodega Logística Juan Viñas", Address = "Cartago, Jiménez" },
                new Warehouse { IdWarehouse = 51, WarehouseName = "Bodega Heredia Central", Address = "Heredia, Heredia" },
                new Warehouse { IdWarehouse = 52, WarehouseName = "Bodega Metropolitana Barva", Address = "Heredia, Barva" },
                new Warehouse { IdWarehouse = 53, WarehouseName = "Bodega Regional Santo Domingo", Address = "Heredia, Santo Domingo" },
                new Warehouse { IdWarehouse = 54, WarehouseName = "Bodega Industrial Santa Bárbara", Address = "Heredia, Santa Bárbara" },
                new Warehouse { IdWarehouse = 55, WarehouseName = "Bodega Norte San Rafael", Address = "Heredia, San Rafael" },
                new Warehouse { IdWarehouse = 56, WarehouseName = "Bodega Bosque San Isidro", Address = "Heredia, San Isidro" },
                new Warehouse { IdWarehouse = 57, WarehouseName = "Bodega Flores Centro", Address = "Heredia, Flores" },
                new Warehouse { IdWarehouse = 58, WarehouseName = "Bodega Café Belén", Address = "Heredia, Belén" },
                new Warehouse { IdWarehouse = 59, WarehouseName = "Bodega Aurora Sarapiquí", Address = "Heredia, Sarapiquí" },
                new Warehouse { IdWarehouse = 60, WarehouseName = "Bodega Logística Horquetas", Address = "Heredia, Sarapiquí" },
                new Warehouse { IdWarehouse = 61, WarehouseName = "Bodega Guanacaste Liberia Norte", Address = "Guanacaste, Liberia" },
                new Warehouse { IdWarehouse = 62, WarehouseName = "Bodega Pampa Nicoya", Address = "Guanacaste, Nicoya" },
                new Warehouse { IdWarehouse = 63, WarehouseName = "Bodega Chorotega Santa Cruz", Address = "Guanacaste, Santa Cruz" },
                new Warehouse { IdWarehouse = 64, WarehouseName = "Bodega Costera Carrillo", Address = "Guanacaste, Carrillo" },
                new Warehouse { IdWarehouse = 65, WarehouseName = "Bodega Frontera La Cruz", Address = "Guanacaste, La Cruz" },
                new Warehouse { IdWarehouse = 66, WarehouseName = "Bodega Industrial Bagaces", Address = "Guanacaste, Bagaces" },
                new Warehouse { IdWarehouse = 67, WarehouseName = "Bodega Río Cañas", Address = "Guanacaste, Cañas" },
                new Warehouse { IdWarehouse = 68, WarehouseName = "Bodega Logística Abangares", Address = "Guanacaste, Abangares" },
                new Warehouse { IdWarehouse = 69, WarehouseName = "Bodega Norte Tilarán", Address = "Guanacaste, Tilarán" },
                new Warehouse { IdWarehouse = 70, WarehouseName = "Bodega Volcán Upala", Address = "Guanacaste, Upala" },
                new Warehouse { IdWarehouse = 71, WarehouseName = "Bodega Pacífico Puntarenas Centro", Address = "Puntarenas, Puntarenas" },
                new Warehouse { IdWarehouse = 72, WarehouseName = "Bodega Puerto Esparza", Address = "Puntarenas, Esparza" },
                new Warehouse { IdWarehouse = 73, WarehouseName = "Bodega Costanera Montes de Oro", Address = "Puntarenas, Montes de Oro" },
                new Warehouse { IdWarehouse = 74, WarehouseName = "Bodega Sur Osa", Address = "Puntarenas, Osa" },
                new Warehouse { IdWarehouse = 75, WarehouseName = "Bodega Península Golfito", Address = "Puntarenas, Golfito" },
                new Warehouse { IdWarehouse = 76, WarehouseName = "Bodega Regional Coto Brus", Address = "Puntarenas, Coto Brus" },
                new Warehouse { IdWarehouse = 77, WarehouseName = "Bodega Pacífico Central Quepos", Address = "Puntarenas, Quepos" },
                new Warehouse { IdWarehouse = 78, WarehouseName = "Bodega Isla Parrita", Address = "Puntarenas, Parrita" },
                new Warehouse { IdWarehouse = 79, WarehouseName = "Bodega Peninsular Corredores", Address = "Puntarenas, Corredores" },
                new Warehouse { IdWarehouse = 80, WarehouseName = "Bodega Costera Garabito", Address = "Puntarenas, Garabito" },
                new Warehouse { IdWarehouse = 81, WarehouseName = "Bodega Caribe Limón", Address = "Limón, Limón" },
                new Warehouse { IdWarehouse = 82, WarehouseName = "Bodega Atlántica Pococí", Address = "Limón, Pococí" },
                new Warehouse { IdWarehouse = 83, WarehouseName = "Bodega Bananera Siquirres", Address = "Limón, Siquirres" },
                new Warehouse { IdWarehouse = 84, WarehouseName = "Bodega Costera Matina", Address = "Limón, Matina" },
                new Warehouse { IdWarehouse = 85, WarehouseName = "Bodega Caribe Sur Talamanca", Address = "Limón, Talamanca" },
                new Warehouse { IdWarehouse = 86, WarehouseName = "Bodega Portuaria Guácimo", Address = "Limón, Guácimo" },
                new Warehouse { IdWarehouse = 87, WarehouseName = "Bodega Logística Cariari", Address = "Limón, Pococí" },
                new Warehouse { IdWarehouse = 88, WarehouseName = "Bodega Regional Bratsi", Address = "Limón, Talamanca" },
                new Warehouse { IdWarehouse = 89, WarehouseName = "Bodega Industrial Batán", Address = "Limón, Matina" },
                new Warehouse { IdWarehouse = 90, WarehouseName = "Bodega Atlántico Centro", Address = "Limón, Limón" },
                new Warehouse { IdWarehouse = 91, WarehouseName = "Bodega Valle Central Montes de Oca", Address = "San José, Montes de Oca" },
                new Warehouse { IdWarehouse = 92, WarehouseName = "Bodega Empresarial Vásquez de Coronado", Address = "San José, Vásquez de Coronado" },
                new Warehouse { IdWarehouse = 93, WarehouseName = "Bodega Comercial Acosta", Address = "San José, Acosta" },
                new Warehouse { IdWarehouse = 94, WarehouseName = "Bodega Desarrollo Mora", Address = "San José, Mora" },
                new Warehouse { IdWarehouse = 95, WarehouseName = "Bodega Logística Santa Ana", Address = "San José, Santa Ana" },
                new Warehouse { IdWarehouse = 96, WarehouseName = "Bodega Industrial Alajuelita", Address = "San José, Alajuelita" },
                new Warehouse { IdWarehouse = 97, WarehouseName = "Bodega Centro Pérez Zeledón", Address = "San José, Pérez Zeledón" },
                new Warehouse { IdWarehouse = 98, WarehouseName = "Bodega Regional Buenos Aires", Address = "Puntarenas, Buenos Aires" },
                new Warehouse { IdWarehouse = 99, WarehouseName = "Bodega Estratégica San Mateo", Address = "Alajuela, San Mateo" },
                new Warehouse { IdWarehouse = 100, WarehouseName = "Bodega Distribución Río Cuarto", Address = "Alajuela, Río Cuarto" }
            );

            // Province
            modelBuilder.Entity<Province>().HasData(
                new Province
                {
                    IdProvince = 1,
                    ProvinceName = "San José"
                },
                new Province
                {
                    IdProvince = 2,
                    ProvinceName = "Alajuela"
                },
                new Province
                {
                    IdProvince = 3,
                    ProvinceName = "Cartago"
                },
                new Province
                {
                    IdProvince = 4,
                    ProvinceName = "Heredia"
                },
                new Province
                {
                    IdProvince = 5,
                    ProvinceName = "Guanacaste"
                },
                new Province
                {
                    IdProvince = 6,
                    ProvinceName = "Puntarenas"
                },
                new Province
                {
                    IdProvince = 7,
                    ProvinceName = "Limón"
                }
            );

            // PaymentType
            modelBuilder.Entity<PaymentType>().HasData(
            new PaymentType
            {
                IdPaymentType = 1,
                PaymentTypeDescription = "Efectivo"
            },
            new PaymentType
            {
                IdPaymentType = 2,
                PaymentTypeDescription = "Tarjeta de Crédito"
            },
            new PaymentType
            {
                IdPaymentType = 3,
                PaymentTypeDescription = "Tarjeta de Débito"
            },
            new PaymentType
            {
                IdPaymentType = 4,
                PaymentTypeDescription = "Transferencia Bancaria"
            },
            new PaymentType
            {
                IdPaymentType = 5,
                PaymentTypeDescription = "SINPE Móvil"
            },
            new PaymentType

            {
                IdPaymentType = 6,
                PaymentTypeDescription = "PayPal"

            }
);
            // Tax
            modelBuilder.Entity<Tax>().HasData(
            new Tax

            {
                IdTax = 1,
                TaxName = "Impuesto de Ventas",
                Percentage = 13M
            },
            new Tax

            {
                IdTax = 2,
                TaxName = "Impuesto Selectivo",
                Percentage = 15M

            }
            );
            // InvoiceStatus
            modelBuilder.Entity<InvoiceStatus>().HasData(
            new InvoiceStatus

            {
                IdInvoiceStatus = 1,
                InvoiceStatusDescription = "Pendiente"
            },
            new InvoiceStatus

            {
                IdInvoiceStatus = 2,
                InvoiceStatusDescription = "Pagada"
            },
            new InvoiceStatus

            {
                IdInvoiceStatus = 3,
                InvoiceStatusDescription = "Anulada"

            }
            );
            // Role
            modelBuilder.Entity<Role>().HasData(
            new Role

            {
                IdRole = 1,
                RoleName = "Administrador",
            },
            new Role

            {
                IdRole = 2,
                RoleName = "Vendedor",
            },
            new Role

            {
                IdRole = 3,
                RoleName = "Cliente",

            }
             );
        }
	}
}