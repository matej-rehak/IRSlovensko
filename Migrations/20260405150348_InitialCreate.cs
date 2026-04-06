using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IRSlovensko.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Osoby",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Typ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Meno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Priezvisko = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitulPredMenom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitulZaMenom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RodneCislo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatumNarodenia = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatneObcianstvo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ObchodneMeno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ico = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Register = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CisloRegistracie = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PravnaForma = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ulica = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupisneCislo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrientacneCislo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Obec = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Psc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Krajina = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Iban = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Swift = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Osoby", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Spravcovia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Znacka = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DatumZapisu = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Meno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Priezvisko = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitulPredMenom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitulZaMenom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatumNarodenia = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ObchodneMeno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ico = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ulica = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupisneCislo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrientacneCislo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Obec = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Psc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Krajina = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spravcovia", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sudy",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nazov = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sudy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Konania",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Typ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpisovaZnackaSudu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpisovaZnackaSpravcu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SudId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SpravcaId = table.Column<int>(type: "int", nullable: true),
                    DlznikId = table.Column<int>(type: "int", nullable: true),
                    Sudca = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatumZacatiaKonania = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DatumZacatiaProcesu = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TypSpravcu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypPrideleniaSpravcu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypKonaniaPodlaUzemnejPlatnosti = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MalyKonkurz = table.Column<bool>(type: "bit", nullable: true),
                    DatumPovoleniaOddlzenia = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DatumZavedeniaDozornejSpravy = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Konania", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Konania_Osoby_DlznikId",
                        column: x => x.DlznikId,
                        principalTable: "Osoby",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Konania_Spravcovia_SpravcaId",
                        column: x => x.SpravcaId,
                        principalTable: "Spravcovia",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Konania_Sudy_SudId",
                        column: x => x.SudId,
                        principalTable: "Sudy",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Navrhovatelia",
                columns: table => new
                {
                    KonanieId = table.Column<long>(type: "bigint", nullable: false),
                    OsobaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Navrhovatelia", x => new { x.KonanieId, x.OsobaId });
                    table.ForeignKey(
                        name: "FK_Navrhovatelia_Konania_KonanieId",
                        column: x => x.KonanieId,
                        principalTable: "Konania",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Navrhovatelia_Osoby_OsobaId",
                        column: x => x.OsobaId,
                        principalTable: "Osoby",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Konania_DlznikId",
                table: "Konania",
                column: "DlznikId");

            migrationBuilder.CreateIndex(
                name: "IX_Konania_SpravcaId",
                table: "Konania",
                column: "SpravcaId");

            migrationBuilder.CreateIndex(
                name: "IX_Konania_SudId",
                table: "Konania",
                column: "SudId");

            migrationBuilder.CreateIndex(
                name: "IX_Navrhovatelia_OsobaId",
                table: "Navrhovatelia",
                column: "OsobaId");

            migrationBuilder.CreateIndex(
                name: "IX_Osoba_Ico",
                table: "Osoby",
                column: "Ico");

            migrationBuilder.CreateIndex(
                name: "IX_Spravca_Znacka",
                table: "Spravcovia",
                column: "Znacka",
                unique: true,
                filter: "[Znacka] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Navrhovatelia");

            migrationBuilder.DropTable(
                name: "Konania");

            migrationBuilder.DropTable(
                name: "Osoby");

            migrationBuilder.DropTable(
                name: "Spravcovia");

            migrationBuilder.DropTable(
                name: "Sudy");
        }
    }
}
