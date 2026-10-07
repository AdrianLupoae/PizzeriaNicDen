using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzeriaNicDen.Migrations
{
    /// <inheritdoc />
    public partial class InitialSqlite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pizze",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nume = table.Column<string>(type: "TEXT", nullable: false),
                    Ingrediente = table.Column<string>(type: "TEXT", nullable: false),
                    CaleImagine = table.Column<string>(type: "TEXT", nullable: false),
                    Pret30cm = table.Column<decimal>(type: "TEXT", nullable: false),
                    ValoriNutritionale30cm = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pizze", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PizzeFamily",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Pret50cm = table.Column<decimal>(type: "TEXT", nullable: false),
                    ValoriNutritionale50cm = table.Column<string>(type: "TEXT", nullable: false),
                    PizzaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PizzeFamily", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PizzeFamily_Pizze_PizzaId",
                        column: x => x.PizzaId,
                        principalTable: "Pizze",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PizzeFamily_PizzaId",
                table: "PizzeFamily",
                column: "PizzaId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PizzeFamily");

            migrationBuilder.DropTable(
                name: "Pizze");
        }
    }
}
