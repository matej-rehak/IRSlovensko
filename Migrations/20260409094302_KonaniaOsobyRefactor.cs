using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IRSlovensko.Migrations
{
    /// <inheritdoc />
    public partial class KonaniaOsobyRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Konania_Osoby_DlznikId",
                table: "Konania");

            migrationBuilder.DropForeignKey(
                name: "FK_Konania_Spravcovia_SpravcaId",
                table: "Konania");

            migrationBuilder.DropTable(
                name: "Navrhovatelia");

            migrationBuilder.DropTable(
                name: "Osoby");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Spravcovia",
                table: "Spravcovia");

            migrationBuilder.DropIndex(
                name: "IX_Spravca_Znacka",
                table: "Spravcovia");

            migrationBuilder.DropIndex(
                name: "IX_Konania_DlznikId",
                table: "Konania");

            migrationBuilder.DropIndex(
                name: "IX_Konania_SpravcaId",
                table: "Konania");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Spravcovia");

            migrationBuilder.DropColumn(
                name: "DlznikId",
                table: "Konania");

            migrationBuilder.DropColumn(
                name: "SpravcaId",
                table: "Konania");

            migrationBuilder.AlterColumn<string>(
                name: "Znacka",
                table: "Spravcovia",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZnackaSpravcu",
                table: "Konania",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Spravcovia",
                table: "Spravcovia",
                column: "Znacka");

            migrationBuilder.CreateTable(
                name: "KonaniaOsoby",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdKonania = table.Column<long>(type: "bigint", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_KonaniaOsoby", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KonaniaOsoby_Konania_IdKonania",
                        column: x => x.IdKonania,
                        principalTable: "Konania",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Konania_ZnackaSpravcu",
                table: "Konania",
                column: "ZnackaSpravcu");

            migrationBuilder.CreateIndex(
                name: "IX_KonaniaOsoby_IdKonania",
                table: "KonaniaOsoby",
                column: "IdKonania");

            migrationBuilder.CreateIndex(
                name: "IX_KonanieOsoba_Ico",
                table: "KonaniaOsoby",
                column: "Ico");

            migrationBuilder.AddForeignKey(
                name: "FK_Konania_Spravcovia_ZnackaSpravcu",
                table: "Konania",
                column: "ZnackaSpravcu",
                principalTable: "Spravcovia",
                principalColumn: "Znacka");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Konania_Spravcovia_ZnackaSpravcu",
                table: "Konania");

            migrationBuilder.DropTable(
                name: "KonaniaOsoby");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Spravcovia",
                table: "Spravcovia");

            migrationBuilder.DropIndex(
                name: "IX_Konania_ZnackaSpravcu",
                table: "Konania");

            migrationBuilder.DropColumn(
                name: "ZnackaSpravcu",
                table: "Konania");

            migrationBuilder.AlterColumn<string>(
                name: "Znacka",
                table: "Spravcovia",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Spravcovia",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "DlznikId",
                table: "Konania",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpravcaId",
                table: "Konania",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Spravcovia",
                table: "Spravcovia",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Osoby",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CisloRegistracie = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatumNarodenia = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Iban = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ico = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Krajina = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Meno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ObchodneMeno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Obec = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrientacneCislo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PravnaForma = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Priezvisko = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Psc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Register = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RodneCislo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatneObcianstvo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupisneCislo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Swift = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitulPredMenom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitulZaMenom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Typ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ulica = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Osoby", x => x.Id);
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
                name: "IX_Spravca_Znacka",
                table: "Spravcovia",
                column: "Znacka",
                unique: true,
                filter: "[Znacka] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Konania_DlznikId",
                table: "Konania",
                column: "DlznikId");

            migrationBuilder.CreateIndex(
                name: "IX_Konania_SpravcaId",
                table: "Konania",
                column: "SpravcaId");

            migrationBuilder.CreateIndex(
                name: "IX_Navrhovatelia_OsobaId",
                table: "Navrhovatelia",
                column: "OsobaId");

            migrationBuilder.CreateIndex(
                name: "IX_Osoba_Ico",
                table: "Osoby",
                column: "Ico");

            migrationBuilder.AddForeignKey(
                name: "FK_Konania_Osoby_DlznikId",
                table: "Konania",
                column: "DlznikId",
                principalTable: "Osoby",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Konania_Spravcovia_SpravcaId",
                table: "Konania",
                column: "SpravcaId",
                principalTable: "Spravcovia",
                principalColumn: "Id");
        }
    }
}
