using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814

namespace IRSlovensko.Migrations
{
    /// <inheritdoc />
    public partial class AddCiselniky : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "CSIRKonaniaDovodUkonceniaProcesu");
            migrationBuilder.DropTable(name: "CSIRKonaniaOsobyRole");
            migrationBuilder.DropTable(name: "CSIRKonaniaOsobyTyp");
            migrationBuilder.DropTable(name: "CSIRKonaniaStavKonania");
            migrationBuilder.DropTable(name: "CSIRKonaniaTyp");
            migrationBuilder.DropTable(name: "CSIRKonaniaTypPrideleniaSpravcu");
            migrationBuilder.DropTable(name: "CSIRKonaniaTypSpravcu");
            migrationBuilder.DropTable(name: "CSIRVerejneOznamyDruhPodania");
            migrationBuilder.DropTable(name: "CSIRVerejneOznamyKonanieTyp");
            migrationBuilder.DropTable(name: "CSIRVerejneOznamyOznamTyp");

            migrationBuilder.CreateTable(
                name: "CSIRKonaniaDovodUkonceniaProcesu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazov = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table => table.PrimaryKey("PK_CSIRKonaniaDovodUkonceniaProcesu", x => x.Id));

            migrationBuilder.CreateTable(
                name: "CSIRKonaniaOsobyRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazov = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table => table.PrimaryKey("PK_CSIRKonaniaOsobyRole", x => x.Id));

            migrationBuilder.CreateTable(
                name: "CSIRKonaniaOsobyTyp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazov = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table => table.PrimaryKey("PK_CSIRKonaniaOsobyTyp", x => x.Id));

            migrationBuilder.CreateTable(
                name: "CSIRKonaniaStavKonania",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazov = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table => table.PrimaryKey("PK_CSIRKonaniaStavKonania", x => x.Id));

            migrationBuilder.CreateTable(
                name: "CSIRKonaniaTyp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazov = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table => table.PrimaryKey("PK_CSIRKonaniaTyp", x => x.Id));

            migrationBuilder.CreateTable(
                name: "CSIRKonaniaTypPrideleniaSpravcu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazov = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table => table.PrimaryKey("PK_CSIRKonaniaTypPrideleniaSpravcu", x => x.Id));

            migrationBuilder.CreateTable(
                name: "CSIRKonaniaTypSpravcu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazov = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table => table.PrimaryKey("PK_CSIRKonaniaTypSpravcu", x => x.Id));

            migrationBuilder.CreateTable(
                name: "CSIRVerejneOznamyDruhPodania",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazov = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table => table.PrimaryKey("PK_CSIRVerejneOznamyDruhPodania", x => x.Id));

            migrationBuilder.CreateTable(
                name: "CSIRVerejneOznamyKonanieTyp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazov = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table => table.PrimaryKey("PK_CSIRVerejneOznamyKonanieTyp", x => x.Id));

            migrationBuilder.CreateTable(
                name: "CSIRVerejneOznamyOznamTyp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazov = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table => table.PrimaryKey("PK_CSIRVerejneOznamyOznamTyp", x => x.Id));

            migrationBuilder.InsertData(
                table: "CSIRKonaniaTyp",
                columns: new[] { "Id", "Nazov" },
                values: new object[,]
                {
                    { 1, "Likvidácia" },
                    { 2, "Iné" },
                    { 3, "Reštrukturalizácia" },
                    { 4, "Oddĺženie - splátkový kalendár" },
                    { 5, "Oddĺženie - konkurz" },
                    { 6, "Malý konkurz" },
                    { 7, "Konkurz" },
                    { 8, "Verejná preventívna reštrukturalizácia" }
                });

            migrationBuilder.InsertData(
                table: "CSIRKonaniaTypSpravcu",
                columns: new[] { "Id", "Nazov" },
                values: new object[,]
                {
                    { 1, "Riadny" },
                    { 2, "Dozorný" },
                    { 3, "Predbežný" }
                });

            migrationBuilder.InsertData(
                table: "CSIRKonaniaTypPrideleniaSpravcu",
                columns: new[] { "Id", "Nazov" },
                values: new object[,]
                {
                    { 1, "Vymenovaný" },
                    { 2, "Generovaný" }
                });

            migrationBuilder.InsertData(
                table: "CSIRKonaniaStavKonania",
                columns: new[] { "Id", "Nazov" },
                values: new object[,]
                {
                    { 1, "Prebiehajúce konanie" },
                    { 2, "Prerušené konanie" },
                    { 3, "Skončený proces" },
                    { 4, "Začaté konanie" },
                    { 5, "Začatý proces konania" },
                    { 6, "Zastavené konanie" }
                });

            migrationBuilder.InsertData(
                table: "CSIRKonaniaDovodUkonceniaProcesu",
                columns: new[] { "Id", "Nazov" },
                values: new object[,]
                {
                    { 1, "Inak" },
                    { 2, "Iný dôvod" },
                    { 3, "Nedá sa zostaviť splátkový kalendár správcom" },
                    { 4, "Nedostatok majetku" },
                    { 5, "Nesplnenie podmienok" },
                    { 6, "Odmietnutie" },
                    { 7, "Odsúhlasený plán" },
                    { 8, "Osvedčená platobná schopnosť" },
                    { 9, "Potvrdenie plánu súdom" },
                    { 10, "Povolenie reštrukturalizácie" },
                    { 11, "Skončené oddĺženie" },
                    { 12, "Späťvzatie" },
                    { 13, "Splnenie konečného rozvrhu" },
                    { 14, "Splnenie rozvrhu výťažku" },
                    { 15, "Určený splátkový kalendár" },
                    { 16, "Vyhlásenie konkurzu" },
                    { 17, "Zaplatenie splatných pohľadávok" },
                    { 18, "Žiadny veriteľ" }
                });

            migrationBuilder.InsertData(
                table: "CSIRVerejneOznamyOznamTyp",
                columns: new[] { "Id", "Nazov" },
                values: new object[,]
                {
                    { 1, "Oznám súdu" },
                    { 2, "Oznám správcu" }
                });

            migrationBuilder.InsertData(
                table: "CSIRVerejneOznamyKonanieTyp",
                columns: new[] { "Id", "Nazov" },
                values: new object[,]
                {
                    { 1, "Konkurz" },
                    { 2, "Malý konkurz" },
                    { 3, "Reštrukturalizácia" },
                    { 4, "Iné konania" },
                    { 5, "Oddĺženie - konkurz" },
                    { 6, "Oddĺženie - splátkový kalendár" },
                    { 7, "Likvidácia" },
                    { 8, "Verejná preventívna reštrukturalizácia" }
                });

            migrationBuilder.InsertData(
                table: "CSIRVerejneOznamyDruhPodania",
                columns: new[] { "Id", "Nazov" },
                values: new object[,]
                {
                    { 1, "Oznám o mieste a čase nahliadnutia do spisu" },
                    { 2, "Oznám o zvolaní schôdze veriteľov" },
                    { 3, "Zápisnica zo zasadnutia veriteľského výboru" },
                    { 4, "Oznámenia súvisiace s dražbou majetku" },
                    { 5, "Súpis všeobecnej podstaty" },
                    { 6, "Súpis oddelenej podstaty" },
                    { 7, "Zverejnenie návrhu čiastkového rozvrhu (všeobecná podstata)" },
                    { 8, "Zverejnenie návrhu konečného výťažku (všeobecná podstata)" },
                    { 9, "Doplnenie súpisu všeobecnej podstaty" },
                    { 10, "Doplnenie súpisu oddelenej podstaty" },
                    { 11, "Preradenie súpisovej zložky" },
                    { 12, "Pripísanie poznámky o spornom zápise" },
                    { 13, "Doplnenie alebo zmena poznámky o spornom zápise" },
                    { 14, "Výmaz poznámky o spornom zápise" },
                    { 15, "Vylúčenie súpisovej zložky majetku zo súpisu" },
                    { 16, "Iné zverejnenie" },
                    { 17, "Výzva zahraničným veriteľom na prihlásenie pohľadávok" },
                    { 18, "Oznámenie čísla bankového účtu pre potreby popierania pohľadávok" },
                    { 19, "Oznámenie o doručení prihlášky po uplynutí základnej prihlasovacej lehoty" },
                    { 20, "Oznámenia súvisiace so speňažovaním majetku" },
                    { 21, "Oznámenie o zostavení zoznamu pohľadávok (všeobecná podstata)" },
                    { 22, "Oznámenie o zostavení zoznamu pohľadávok (oddelená podstata)" },
                    { 23, "Zverejnenie návrhu čiastkového rozvrhu (oddelená podstata)" },
                    { 24, "Zverejnenie návrhu konečného rozvrhu (oddelená podstata)" },
                    { 25, "Zápisnica zo zasadnutia so zástupcom veriteľov" },
                    { 26, "Oznám o zostavení návrhu splátkového kalendára" },
                    { 27, "Oznám o nemožnosti zostaviť návrh splátkového kalendára" },
                    { 28, "Oznám o zrušení konkurzu - splnenie rozvrhu výťažku" },
                    { 29, "Oznám o zrušení konkurzu - podstata nepokryje náklady" },
                    { 30, "Oznám o zrušení konkurzu - žiadny veriteľ do 90 dní" },
                    { 31, "Oznám o zrušení konkurzu - zánik postavenia veriteľov" },
                    { 32, "Oznám o skončení konania o návrhu na určenie SK" },
                    { 33, "Oznám o uplatnení námietky voči zápisu majetku do súpisu" },
                    { 34, "Prebiehajúca likvidácia" },
                    { 35, "Likvidácia - prihlasovanie pohľadávok" },
                    { 36, "Likvidácia - vyhotovenie zoznamu pohľadávok" },
                    { 37, "Likvidácia - vyhotovenie zoznamu majetku" },
                    { 38, "Podanie návrhu na vyhlásenie konkurzu (likvidátor)" },
                    { 39, "Prerušená likvidácia - vyhlásený konkurz" },
                    { 40, "Prerušená likvidácia" },
                    { 41, "Skončené oznámením likvidátora" },
                    { 42, "Schválenie účtovnej závierky a konečnej správy" },
                    { 43, "Prebiehajúca dodatočná likvidácia" },
                    { 44, "Dodatočná likvidácia - prihlasovanie pohľadávok" },
                    { 45, "Dodatočná likvidácia - vyhotovenie zoznamu pohľadávok" },
                    { 46, "Dodatočná likvidácia - vyhotovenie zoznamu majetku" },
                    { 47, "Dodatočná likvidácia - podanie návrhu na vyhlásenie konkurzu" },
                    { 48, "Dodatočná likvidácia - prerušené, vyhlásený konkurz" },
                    { 49, "Dodatočná likvidácia - prerušené" },
                    { 50, "Dodatočná likvidácia - skončené oznámením likvidátora" },
                    { 51, "Dodatočná likvidácia - schválenie účtovnej závierky" },
                    { 52, "Zvolanie schvaľovacej schôdze veriteľov po schválení plánu" },
                    { 53, "Správa o postupe speňažovania" },
                    { 54, "Zápisnica zo schôdze veriteľov" },
                    { 55, "Zápisnica zo schvaľovacej schôdze veriteľov" },
                    { 56, "Zvolanie schôdze veriteľského výboru na schválenie plánu" },
                    { 57, "Záznam z online schôdze veriteľov" },
                    { 58, "Oznámenie o zverejnení súpisu majetku" },
                    { 59, "Ukončenie prihlasovania pohľadávok" },
                    { 60, "Ukončenie prieskumu pohľadávok" },
                    { 61, "Nedefinované" }
                });

            migrationBuilder.InsertData(
                table: "CSIRKonaniaOsobyTyp",
                columns: new[] { "Id", "Nazov" },
                values: new object[,]
                {
                    { 1, "Fyzická osoba" },
                    { 2, "Fyzická osoba - podnikateľ" },
                    { 3, "Právnická osoba" }
                });

            migrationBuilder.InsertData(
                table: "CSIRKonaniaOsobyRole",
                columns: new[] { "Id", "Nazov" },
                values: new object[,]
                {
                    { 1, "Dlžník" },
                    { 2, "Navrhovateľ" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "CSIRKonaniaDovodUkonceniaProcesu");
            migrationBuilder.DropTable(name: "CSIRKonaniaOsobyRole");
            migrationBuilder.DropTable(name: "CSIRKonaniaOsobyTyp");
            migrationBuilder.DropTable(name: "CSIRKonaniaStavKonania");
            migrationBuilder.DropTable(name: "CSIRKonaniaTyp");
            migrationBuilder.DropTable(name: "CSIRKonaniaTypPrideleniaSpravcu");
            migrationBuilder.DropTable(name: "CSIRKonaniaTypSpravcu");
            migrationBuilder.DropTable(name: "CSIRVerejneOznamyDruhPodania");
            migrationBuilder.DropTable(name: "CSIRVerejneOznamyKonanieTyp");
            migrationBuilder.DropTable(name: "CSIRVerejneOznamyOznamTyp");

            migrationBuilder.CreateTable(
                name: "CSIRKonaniaDovodUkonceniaProcesu",
                columns: table => new
                {
                    Kod = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nazov = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table => table.PrimaryKey("PK_CSIRKonaniaDovodUkonceniaProcesu", x => x.Kod));

            migrationBuilder.CreateTable(
                name: "CSIRKonaniaOsobyRole",
                columns: table => new
                {
                    Kod = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nazov = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table => table.PrimaryKey("PK_CSIRKonaniaOsobyRole", x => x.Kod));

            migrationBuilder.CreateTable(
                name: "CSIRKonaniaOsobyTyp",
                columns: table => new
                {
                    Kod = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nazov = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table => table.PrimaryKey("PK_CSIRKonaniaOsobyTyp", x => x.Kod));

            migrationBuilder.CreateTable(
                name: "CSIRKonaniaStavKonania",
                columns: table => new
                {
                    Kod = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nazov = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table => table.PrimaryKey("PK_CSIRKonaniaStavKonania", x => x.Kod));

            migrationBuilder.CreateTable(
                name: "CSIRKonaniaTyp",
                columns: table => new
                {
                    Kod = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nazov = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table => table.PrimaryKey("PK_CSIRKonaniaTyp", x => x.Kod));

            migrationBuilder.CreateTable(
                name: "CSIRKonaniaTypPrideleniaSpravcu",
                columns: table => new
                {
                    Kod = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nazov = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table => table.PrimaryKey("PK_CSIRKonaniaTypPrideleniaSpravcu", x => x.Kod));

            migrationBuilder.CreateTable(
                name: "CSIRKonaniaTypSpravcu",
                columns: table => new
                {
                    Kod = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nazov = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table => table.PrimaryKey("PK_CSIRKonaniaTypSpravcu", x => x.Kod));

            migrationBuilder.CreateTable(
                name: "CSIRVerejneOznamyDruhPodania",
                columns: table => new
                {
                    Kod = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nazov = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table => table.PrimaryKey("PK_CSIRVerejneOznamyDruhPodania", x => x.Kod));

            migrationBuilder.CreateTable(
                name: "CSIRVerejneOznamyKonanieTyp",
                columns: table => new
                {
                    Kod = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nazov = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table => table.PrimaryKey("PK_CSIRVerejneOznamyKonanieTyp", x => x.Kod));

            migrationBuilder.CreateTable(
                name: "CSIRVerejneOznamyOznamTyp",
                columns: table => new
                {
                    Kod = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nazov = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table => table.PrimaryKey("PK_CSIRVerejneOznamyOznamTyp", x => x.Kod));
        }
    }
}
