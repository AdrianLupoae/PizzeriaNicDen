using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzeriaNicDen.Migrations
{
    /// <inheritdoc />
    public partial class AdaugaAnunturi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Anunturi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Titlu = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Mesaj = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    DataInceput = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataSfarsit = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Activ = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataCreare = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anunturi", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Anunturi");
        }
    }
}
