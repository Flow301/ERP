using ERP.Domain.Entities;
using ERP.Infraestructure.Data.Seed;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ERP.Infraestructure.Data.Seed;


namespace ERP.Infraestructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categorie { get; set; }
    public DbSet<CreditCard> CreditCard { get; set; }
    public DbSet<Customer> Customer { get; set; }
    public DbSet<Invoice> Invoice { get; set; }
    public DbSet<InvoiceItem> InvoiceItem { get; set; }
    public DbSet<InvoiceStatus> InvoiceStatuse { get; set; }
    public DbSet<PaymentType> PaymentTypes { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<ProductSupplier> ProductSupplier { get; set; }
    public DbSet<Province> Province { get; set; }
    public DbSet<Role> Role { get; set; }
    public DbSet<Supplier> Supplier { get; set; }
    public DbSet<Tax> Taxe { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<Warehouse> Warehouse { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Category
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category", "dbo");

            entity.HasKey(e => e.IdCategory)
                .HasName("PK_CategoriaRopa");

            entity.HasIndex(e => e.CategoryName, "UQ_CategoriaRopa_DescripcionCategoria");

            entity.Property(e => e.IdCategory)
                .ValueGeneratedNever();

            entity.Property(e => e.CategoryName)
                .HasMaxLength(80);
        });

        // CreditCard
        modelBuilder.Entity<CreditCard>(entity =>
        {
            entity.HasKey(e => e.IdCreditCard)
                .HasName("PK_Tarjeta");

            entity.HasIndex(e => e.CreditCardName, "UQ_Tarjeta_DescripcionTarjeta")
                .IsUnique();

            entity.Property(e => e.IdCreditCard)
                .ValueGeneratedNever();

            entity.Property(e => e.CreditCardName)
                .HasMaxLength(50);
        });

        // Province
        modelBuilder.Entity<Province>(entity =>
        {
            entity.HasKey(e => e.IdProvince)
                .HasName("PK_Provincia");

            entity.HasIndex(e => e.ProvinceName, "UQ_Provincia_Descripcion")
                .IsUnique();

            entity.Property(e => e.IdProvince)
                .ValueGeneratedNever();

            entity.Property(e => e.ProvinceName)
                .HasMaxLength(50);
        });

        // Customer
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.IdCustomer)
                .HasName("PK_Cliente");

            entity.HasIndex(e => e.Email, "UQ_Cliente_Email")
                .IsUnique();

            entity.Property(e => e.IdCustomer)
                .HasMaxLength(15)
                .IsFixedLength();

            entity.Property(e => e.FirstName)
                .HasMaxLength(40);

            entity.Property(e => e.Surname)
                .HasMaxLength(40);

            entity.Property(e => e.LastName)
                .HasMaxLength(40);

            entity.Property(e => e.DateOfBirth)
                .HasColumnType("date");

            entity.Property(e => e.Email)
                .HasMaxLength(100);

            entity.Property(e => e.Phone)
                .HasMaxLength(20);

            entity.Property(e => e.Address)
                .HasMaxLength(200);

            entity.HasOne(d => d.Province)
                .WithMany(p => p.Customers)
                .HasForeignKey(d => d.IdProvince)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cliente_Provincia");
        });

        // InvoiceStatus
        modelBuilder.Entity<InvoiceStatus>(entity =>
        {
            entity.HasKey(e => e.IdInvoiceStatus)
                .HasName("PK_EstadoFactura");

            entity.HasIndex(e => e.InvoiceStatusDescription, "UQ_EstadoFactura_DescripcionEstado")
                .IsUnique();

            entity.Property(e => e.IdInvoiceStatus)
                .ValueGeneratedNever();

            entity.Property(e => e.InvoiceStatusDescription)
                .HasMaxLength(40);
        });

        // PaymentType
        modelBuilder.Entity<PaymentType>(entity =>
        {
            entity.HasKey(e => e.IdPaymentType)
                .HasName("PK_TipoPago");

            entity.HasIndex(e => e.PaymentTypeDescription, "UQ_TipoPago_DescripcionTipoPago")
                .IsUnique();

            entity.Property(e => e.IdPaymentType)
                .ValueGeneratedNever();

            entity.Property(e => e.PaymentTypeDescription)
                .HasMaxLength(40);
        });

        // Role
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole)
                .HasName("PK_Rol");

            entity.HasIndex(e => e.RoleName, "UQ_Rol_DescripcionRol")
                .IsUnique();

            entity.Property(e => e.IdRole)
                .ValueGeneratedNever();

            entity.Property(e => e.RoleName)
                .HasMaxLength(30);
        });

        // User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser)
                .HasName("PK_Usuario");

            entity.HasIndex(e => e.Email, "UQ_Usuario_Email")
                .IsUnique();

            entity.HasIndex(e => e.Login, "UQ_Usuario_Login")
                .IsUnique();

            entity.Property(e => e.IdUser)
                .ValueGeneratedNever();

            entity.Property(e => e.Login)
                .HasMaxLength(50);

            entity.Property(e => e.Password)
                .HasMaxLength(255);

            entity.Property(e => e.FullName)
                .HasMaxLength(80);

            entity.Property(e => e.Email)
                .HasMaxLength(100);

            entity.Property(e => e.Status)
                .HasSentinel(true);

            entity.HasOne(d => d.Role)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_Rol");
        });

        // Invoice
        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.IdInvoice)
                .HasName("PK_FacturaEncabezado");

            entity.Property(e => e.IdInvoice)
                .ValueGeneratedNever();

            entity.Property(e => e.IdCustomer)
                .HasMaxLength(15)
                .IsFixedLength();

            entity.Property(e => e.CreditCardNumber)
                .HasMaxLength(20);

            entity.Property(e => e.InvoiceDate)
                .HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.Customer)
                .WithMany(p => p.Invoices)
                .HasForeignKey(d => d.IdCustomer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FacturaEncabezado_Cliente");

            entity.HasOne(d => d.CreditCard)
                .WithMany(p => p.Invoices)
                .HasForeignKey(d => d.IdCreditCard)
                .HasConstraintName("FK_FacturaEncabezado_Tarjeta");

            entity.HasOne(d => d.InvoiceStatus)
                .WithMany(p => p.Invoices)
                .HasForeignKey(d => d.IdInvoiceStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FacturaEncabezado_EstadoFactura");

            entity.HasOne(d => d.PaymentType)
                .WithMany(p => p.Invoices)
                .HasForeignKey(d => d.IdPaymentType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FacturaEncabezado_TipoPago");

            entity.HasOne(d => d.User)
                .WithMany(p => p.Invoices)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FacturaEncabezado_Usuario");
        });

        // Warehouse
        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.IdWarehouse)
                .HasName("PK_Bodega");

            entity.HasIndex(e => e.WarehouseName, "UQ_Bodega_DescripcionBodega")
                .IsUnique();

            entity.Property(e => e.IdWarehouse)
                .ValueGeneratedNever();

            entity.Property(e => e.WarehouseName)
                .HasMaxLength(80);

            entity.Property(e => e.Address)
                .HasMaxLength(120);
        });

        // Product
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdProduct)
                .HasName("PK_Ropa");

            entity.Property(e => e.IdProduct).ValueGeneratedNever();

            entity.Property(e => e.ProductName).HasMaxLength(100);

            entity.Property(e => e.Description).HasMaxLength(250);

            entity.Property(e => e.Price)
                .HasColumnType("decimal(18, 2)");

            entity.Property(e => e.Image)
                .HasMaxLength(50);

            entity.Property(e => e.InsertDate)
                .HasDefaultValueSql("(sysdatetime())");

            entity.Property(e => e.Status)
                .HasSentinel(true);

            entity.HasOne(d => d.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(d => d.IdCategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ropa_CategoriaRopa");

            entity.HasOne(d => d.Warehouse)
                .WithMany(p => p.Products)
                .HasForeignKey(d => d.IdWarehouse)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ropa_Bodega");
        });

        // Tax
        modelBuilder.Entity<Tax>(entity =>
        {
            entity.HasKey(e => e.IdTax)
                .HasName("PK_Impuesto");

            entity.HasIndex(e => e.TaxName, "UQ_Impuesto_DescripcionImpuesto")
                .IsUnique();

            entity.Property(e => e.IdTax)
                .ValueGeneratedNever();

            entity.Property(e => e.TaxName)
                .HasMaxLength(50);

            entity.Property(e => e.Percentage)
                .HasColumnType("decimal(5, 2)");
        });

        // InvoiceItems
        modelBuilder.Entity<InvoiceItem>(entity =>
        {
            entity.HasKey(e => new { e.IdInvoice, e.LineNumber })
                .HasName("PK_InvoiceItems");

            entity.Property(e => e.Price)
                .HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Invoice)
                .WithMany(p => p.InvoiceItems)
                .HasForeignKey(d => d.IdInvoice)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceItems_Invoice");

            entity.HasOne(d => d.Product)
                .WithMany(p => p.InvoiceItems)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FacturaDetalle_Ropa");

            entity.HasOne(d => d.Tax)
                .WithMany(p => p.InvoiceItems)
                .HasForeignKey(d => d.IdTax)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FacturaDetalle_Impuesto");
        });

        // Supplier
        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.IdSupplier)
                .HasName("PK_Proveedor");

            entity.HasIndex(e => e.LegalEntityNumber, "UQ_Proveedor_CedulaJuridica")
                .IsUnique();

            entity.HasIndex(e => e.Email, "UQ_Proveedor_Email")
                .IsUnique();

            entity.Property(e => e.IdSupplier)
                .ValueGeneratedNever();

            entity.Property(e => e.SupplierName)
                .HasMaxLength(100);

            entity.Property(e => e.LegalEntityNumber)
                .HasMaxLength(30);

            entity.Property(e => e.Phone)
                .HasMaxLength(20);

            entity.Property(e => e.Email)
                .HasMaxLength(100);

            entity.Property(e => e.Address)
                .HasMaxLength(200);

            entity.Property(e => e.Status)
                .HasSentinel(true);
        });

        // ProductSupplier (Many-to-Many join table)
        modelBuilder.Entity<ProductSupplier>(entity =>
        {
            entity.HasKey(e => new { e.IdProduct, e.IdSupplier })
                .HasName("PK_RopaProveedor");

            entity.HasOne(d => d.Product)
                .WithMany(p => p.ProductSuppliers)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RopaProveedor_Ropa");

            entity.HasOne(d => d.Supplier)
                .WithMany(p => p.ProductSuppliers)
                .HasForeignKey(d => d.IdSupplier)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RopaProveedor_Proveedor");
        });

        // ... al final de OnModelCreating:
        // Seed maestro de catálogos (HasData)
        modelBuilder.SeedCatalogs();
    }
}