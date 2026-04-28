using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IRSlovensko.Migrations
{
    /// <inheritdoc />
    public partial class AddStatistika : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Statistika",
                columns: table => new
                {
                    DatumStahovania = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statistika", x => x.DatumStahovania);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Statistika");
        }
    }
}
