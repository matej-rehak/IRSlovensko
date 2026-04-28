using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IRSlovensko.Migrations
{
    /// <inheritdoc />
    public partial class AddVerejneOznamy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VerejneOznamy",
                columns: table => new
                {
                    OznamId = table.Column<long>(type: "bigint", nullable: false),
                    OznamTyp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SudKod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SudNazov = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpisovaZnackaSudnehoSpisu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KonanieId = table.Column<long>(type: "bigint", nullable: false),
                    KonanieTyp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatumVydania = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerejneOznamy", x => x.OznamId);
                    table.ForeignKey(
                        name: "FK_VerejneOznamy_Konania_KonanieId",
                        column: x => x.KonanieId,
                        principalTable: "Konania",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VerejnyOznam_KonanieId",
                table: "VerejneOznamy",
                column: "KonanieId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VerejneOznamy");
        }
    }
}
