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