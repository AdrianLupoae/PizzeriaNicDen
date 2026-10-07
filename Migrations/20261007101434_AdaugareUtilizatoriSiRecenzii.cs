using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzeriaNicDen.Migrations
{
    /// <inheritdoc />
    public partial class AdaugareUtilizatoriSiRecenzii : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Utilizatori",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Nume = table.Column<string>(type: "TEXT", nullable: false),
                    Rol = table.Column<string>(type: "TEXT", nullable: false),
                    NumarLogari = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilizatori", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AprecieriPizze",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UtilizatorId = table.Column<int>(type: "INTEGER", nullable: false),
                    PizzaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AprecieriPizze", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AprecieriPizze_Pizze_PizzaId",
                        column: x => x.PizzaId,
                        principalTable: "Pizze",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AprecieriPizze_Utilizatori_UtilizatorId",
                        column: x => x.UtilizatorId,
                        principalTable: "Utilizatori",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recenzii",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Stele = table.Column<int>(type: "INTEGER", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    DataAdaugare = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UtilizatorId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recenzii", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recenzii_Utilizatori_UtilizatorId",
                        column: x => x.UtilizatorId,
                        principalTable: "Utilizatori",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AprecieriPizze_PizzaId",
                table: "AprecieriPizze",
                column: "PizzaId");

            migrationBuilder.CreateIndex(
                name: "IX_AprecieriPizze_UtilizatorId",
                table: "AprecieriPizze",
                column: "UtilizatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Recenzii_UtilizatorId",
                table: "Recenzii",
                column: "UtilizatorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AprecieriPizze");

            migrationBuilder.DropTable(
                name: "Recenzii");

            migrationBuilder.DropTable(
                name: "Utilizatori");
        }
    }
}
