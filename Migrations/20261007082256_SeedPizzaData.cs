using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PizzeriaNicDen.Migrations
{
    /// <inheritdoc />
    public partial class SeedPizzaData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Pizze",
                columns: new[] { "Id", "CaleImagine", "Ingrediente", "Nume", "Pret30cm", "ValoriNutritionale30cm" },
                values: new object[,]
                {
                    { 1, "/img/margherita.jpg", "Sos de roșii, mozzarella fior di latte, busuioc proaspăt, ulei de măsline", "Margherita", 28.00m, "250 kcal / 100g" },
                    { 2, "/img/diavola.jpg", "Sos de roșii, mozzarella, salam picant, ardei iute jalapeño", "Diavola", 34.00m, "310 kcal / 100g" },
                    { 3, "/img/nicden.jpg", "Sos de roșii, mozzarella, șuncă, ciuperci, măsline, bacon, ardei gras", "Specialitatea Nic&Den", 38.00m, "290 kcal / 100g" }
                });

            migrationBuilder.InsertData(
                table: "PizzeFamily",
                columns: new[] { "Id", "PizzaId", "Pret50cm", "ValoriNutritionale50cm" },
                values: new object[,]
                {
                    { 1, 1, 50.00m, "250 kcal / 100g" },
                    { 2, 3, 65.00m, "290 kcal / 100g" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Pizze",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PizzeFamily",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PizzeFamily",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Pizze",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Pizze",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
