using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ERP.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreWarehouses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Warehouse",
                columns: new[] { "IdWarehouse", "Address", "WarehouseName" },
                values: new object[,]
                {
                    { 11, "San José, San José", "Bodega Central San José" },
                    { 12, "San José, Escazú", "Bodega Metropolitana Escazú" },
                    { 13, "San José, Desamparados", "Bodega Logística Desamparados" },
                    { 14, "Alajuela, Alajuela", "Bodega Regional Alajuela" },
                    { 15, "Alajuela, San Ramón", "Bodega Occidente San Ramón" },
                    { 16, "Cartago, Cartago", "Bodega Industrial Cartago" },
                    { 17, "Cartago, Paraíso", "Bodega Valle de Paraíso" },
                    { 18, "Heredia, Heredia", "Bodega Heredia Norte" },
                    { 19, "Guanacaste, Liberia", "Bodega Guanacaste Liberia" },
                    { 20, "Puntarenas, Puntarenas", "Bodega Pacífico Puntarenas" },
                    { 21, "San José, Tibás", "Bodega Central Tibás" },
                    { 22, "San José, Moravia", "Bodega Logística Moravia" },
                    { 23, "San José, Goicoechea", "Bodega Metropolitana Goicoechea" },
                    { 24, "San José, Curridabat", "Bodega Este Curridabat" },
                    { 25, "San José, Aserrí", "Bodega Sur Aserrí" },
                    { 26, "San José, Puriscal", "Bodega Valle Puriscal" },
                    { 27, "San José, Turrubares", "Bodega Industrial Turrubares" },
                    { 28, "San José, Dota", "Bodega Regional Dota" },
                    { 29, "San José, Tarrazú", "Bodega Montaña Tarrazú" },
                    { 30, "San José, León Cortés", "Bodega Los Santos León Cortés" },
                    { 31, "Alajuela, Grecia", "Bodega Norte Grecia" },
                    { 32, "Alajuela, Naranjo", "Bodega Occidente Naranjo" },
                    { 33, "Alajuela, Palmares", "Bodega Cafetal Palmares" },
                    { 34, "Alajuela, Poás", "Bodega Industrial Poás" },
                    { 35, "Alajuela, Atenas", "Bodega Volcán Atenas" },
                    { 36, "Alajuela, Orotina", "Bodega Regional Orotina" },
                    { 37, "Alajuela, San Carlos", "Bodega Frontera San Carlos" },
                    { 38, "Alajuela, Guatuso", "Bodega Pital Guatuso" },
                    { 39, "Alajuela, Upala", "Bodega Norte Upala" },
                    { 40, "Alajuela, Los Chiles", "Bodega Llanuras Los Chiles" },
                    { 41, "Cartago, Cartago", "Bodega Cartago Centro" },
                    { 42, "Cartago, Oreamuno", "Bodega Valle Oreamuno" },
                    { 43, "Cartago, El Guarco", "Bodega Colonial El Guarco" },
                    { 44, "Cartago, Alvarado", "Bodega Norte Alvarado" },
                    { 45, "Cartago, Jiménez", "Bodega Este Jiménez" },
                    { 46, "Cartago, Turrialba", "Bodega Agrícola Turrialba" },
                    { 47, "Cartago, Paraíso", "Bodega Valle Central Paraíso" },
                    { 48, "Cartago, Alvarado", "Bodega Montaña Cervantes" },
                    { 49, "Cartago, Jiménez", "Bodega Industrial Tucurrique" },
                    { 50, "Cartago, Jiménez", "Bodega Logística Juan Viñas" },
                    { 51, "Heredia, Heredia", "Bodega Heredia Central" },
                    { 52, "Heredia, Barva", "Bodega Metropolitana Barva" },
                    { 53, "Heredia, Santo Domingo", "Bodega Regional Santo Domingo" },
                    { 54, "Heredia, Santa Bárbara", "Bodega Industrial Santa Bárbara" },
                    { 55, "Heredia, San Rafael", "Bodega Norte San Rafael" },
                    { 56, "Heredia, San Isidro", "Bodega Bosque San Isidro" },
                    { 57, "Heredia, Flores", "Bodega Flores Centro" },
                    { 58, "Heredia, Belén", "Bodega Café Belén" },
                    { 59, "Heredia, Sarapiquí", "Bodega Aurora Sarapiquí" },
                    { 60, "Heredia, Sarapiquí", "Bodega Logística Horquetas" },
                    { 61, "Guanacaste, Liberia", "Bodega Guanacaste Liberia Norte" },
                    { 62, "Guanacaste, Nicoya", "Bodega Pampa Nicoya" },
                    { 63, "Guanacaste, Santa Cruz", "Bodega Chorotega Santa Cruz" },
                    { 64, "Guanacaste, Carrillo", "Bodega Costera Carrillo" },
                    { 65, "Guanacaste, La Cruz", "Bodega Frontera La Cruz" },
                    { 66, "Guanacaste, Bagaces", "Bodega Industrial Bagaces" },
                    { 67, "Guanacaste, Cañas", "Bodega Río Cañas" },
                    { 68, "Guanacaste, Abangares", "Bodega Logística Abangares" },
                    { 69, "Guanacaste, Tilarán", "Bodega Norte Tilarán" },
                    { 70, "Guanacaste, Upala", "Bodega Volcán Upala" },
                    { 71, "Puntarenas, Puntarenas", "Bodega Pacífico Puntarenas Centro" },
                    { 72, "Puntarenas, Esparza", "Bodega Puerto Esparza" },
                    { 73, "Puntarenas, Montes de Oro", "Bodega Costanera Montes de Oro" },
                    { 74, "Puntarenas, Osa", "Bodega Sur Osa" },
                    { 75, "Puntarenas, Golfito", "Bodega Península Golfito" },
                    { 76, "Puntarenas, Coto Brus", "Bodega Regional Coto Brus" },
                    { 77, "Puntarenas, Quepos", "Bodega Pacífico Central Quepos" },
                    { 78, "Puntarenas, Parrita", "Bodega Isla Parrita" },
                    { 79, "Puntarenas, Corredores", "Bodega Peninsular Corredores" },
                    { 80, "Puntarenas, Garabito", "Bodega Costera Garabito" },
                    { 81, "Limón, Limón", "Bodega Caribe Limón" },
                    { 82, "Limón, Pococí", "Bodega Atlántica Pococí" },
                    { 83, "Limón, Siquirres", "Bodega Bananera Siquirres" },
                    { 84, "Limón, Matina", "Bodega Costera Matina" },
                    { 85, "Limón, Talamanca", "Bodega Caribe Sur Talamanca" },
                    { 86, "Limón, Guácimo", "Bodega Portuaria Guácimo" },
                    { 87, "Limón, Pococí", "Bodega Logística Cariari" },
                    { 88, "Limón, Talamanca", "Bodega Regional Bratsi" },
                    { 89, "Limón, Matina", "Bodega Industrial Batán" },
                    { 90, "Limón, Limón", "Bodega Atlántico Centro" },
                    { 91, "San José, Montes de Oca", "Bodega Valle Central Montes de Oca" },
                    { 92, "San José, Vásquez de Coronado", "Bodega Empresarial Vásquez de Coronado" },
                    { 93, "San José, Acosta", "Bodega Comercial Acosta" },
                    { 94, "San José, Mora", "Bodega Desarrollo Mora" },
                    { 95, "San José, Santa Ana", "Bodega Logística Santa Ana" },
                    { 96, "San José, Alajuelita", "Bodega Industrial Alajuelita" },
                    { 97, "San José, Pérez Zeledón", "Bodega Centro Pérez Zeledón" },
                    { 98, "Puntarenas, Buenos Aires", "Bodega Regional Buenos Aires" },
                    { 99, "Alajuela, San Mateo", "Bodega Estratégica San Mateo" },
                    { 100, "Alajuela, Río Cuarto", "Bodega Distribución Río Cuarto" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "Warehouse",
                keyColumn: "IdWarehouse",
                keyValue: 100);
        }
    }
}
