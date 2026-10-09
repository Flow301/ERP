using ERP.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
namespace ERP.Infraestructure.Data.Seed
{
    /// <summary>
    /// SEED MAESTRO EN TIEMPO DE EJECUCIÓN (UseSeeding).
    /// Datos indispensables en TODOS los ambientes que requieren lógica:
    /// el primer usuario administrador, con contraseña en hash.
    /// </summary>
    public static class MasterSeeder
    {
        private const string AdminLogin = "admin";
        // Versión síncrona: la usan las herramientas de EF (dotnet ef database update).
        public static void Seed(DbContext context, string adminPassword)
        {
            // Idempotencia: si el admin ya existe, no se hace nada
            if (context.Set<User>().Any(u => u.Login == AdminLogin))
                return;
            context.Set<User>().Add(CreateAdmin(adminPassword));
            context.SaveChanges();
        }
        // Versión asíncrona: la usa la aplicación (Database.MigrateAsync).
        public static async Task SeedAsync(DbContext context, string adminPassword, CancellationToken ct)
        {
            if (await context.Set<User>().AnyAsync(u => u.Login == AdminLogin, ct))
                return;
            context.Set<User>().Add(CreateAdmin(adminPassword));
            await context.SaveChangesAsync(ct);
        }
        private static User CreateAdmin(string adminPassword)
        {
            var admin = new User
            {
                IdUser = 1, // IdUser es ValueGeneratedNever: se asigna
                IdRole = SeedIds.RoleAdministrador,
                Login = AdminLogin,
                FullName = "Administrador General",
                Email = "admin@tienda.com",
                Status = true
            };
            // Nunca texto plano: se guarda el hash (PBKDF2 con salt aleatorio)
            admin.Password = new PasswordHasher<User>().HashPassword(admin, adminPassword);
            return admin;
        }
    }
}