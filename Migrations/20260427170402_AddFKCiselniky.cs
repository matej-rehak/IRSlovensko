using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IRSlovensko.Migrations
{
    /// <inheritdoc />
    public partial class AddFKCiselniky : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Pridať nové int? FK stĺpce
            migrationBuilder.AddColumn<int>(
                name: "TypId",
                table: "Konania",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StavKonaniaId",
                table: "Konania",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DovodUkonceniaProcesuId",
                table: "Konania",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TypSpravcuId",
                table: "Konania",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TypPrideleniaSpravcuId",
                table: "Konania",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TypId",
                table: "KonaniaOsoby",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "KonaniaOsoby",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OznamTypId",
                table: "VerejneOznamy",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KonanieTypId",
                table: "VerejneOznamy",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DruhPodaniaId",
                table: "VerejneOznamy",
                type: "int",
                nullable: true);

            // 2. Dátová migrácia — konverzia existujúcich string hodnôt na FK int ID

            migrationBuilder.Sql(@"
UPDATE Konania SET TypId = CASE Typ
    WHEN 'LIKVIDACIA' THEN 1
    WHEN 'INE' THEN 2
    WHEN 'RESTRUKTURALIZACIA' THEN 3
    WHEN 'ODDLZENIE_SPLATKOVY_KALENDAR' THEN 4
    WHEN 'ODDLZENIE_KONKURZ' THEN 5
    WHEN 'MALYKONKURZ' THEN 6
    WHEN 'KONKURZ' THEN 7
    WHEN 'VPR' THEN 8
    ELSE NULL END");

            migrationBuilder.Sql(@"
UPDATE Konania SET TypSpravcuId = CASE TypSpravcu
    WHEN 'RIADNY' THEN 1
    WHEN 'DOZORNY' THEN 2
    WHEN 'PREDBEZNY' THEN 3
    ELSE NULL END");

            migrationBuilder.Sql(@"
UPDATE Konania SET TypPrideleniaSpravcuId = CASE TypPrideleniaSpravcu
    WHEN 'VYMENOVANY' THEN 1
    WHEN 'GENEROVANY' THEN 2
    ELSE NULL END");

            migrationBuilder.Sql(@"
UPDATE Konania SET StavKonaniaId = CASE StavKonania
    WHEN N'Prebiehajúce konanie' THEN 1
    WHEN N'Prerušené konanie' THEN 2
    WHEN N'Skončený proces' THEN 3
    WHEN N'Začaté konanie' THEN 4
    WHEN N'Začatý proces konania' THEN 5
    WHEN N'Zastavené konanie' THEN 6
    ELSE NULL END");

            migrationBuilder.Sql(@"
UPDATE Konania SET DovodUkonceniaProcesuId = CASE DovodUkonceniaProcesu
    WHEN N'Inak' THEN 1
    WHEN N'Iný dôvod' THEN 2
    WHEN N'Nedá sa zostaviť splátkový kalendár správcom' THEN 3
    WHEN N'Nedostatok majetku' THEN 4
    WHEN N'Nesplnenie podmienok' THEN 5
    WHEN N'Odmietnutie' THEN 6
    WHEN N'Odsúhlasený plán' THEN 7
    WHEN N'Osvedčená platobná schopnosť' THEN 8
    WHEN N'Potvrdenie plánu súdom' THEN 9
    WHEN N'Povolenie reštrukturalizácie' THEN 10
    WHEN N'Skončené oddĺženie' THEN 11
    WHEN N'Späťvzatie' THEN 12
    WHEN N'Splnenie konečného rozvrhu' THEN 13
    WHEN N'Splnenie rozvrhu výťažku' THEN 14
    WHEN N'Určený splátkový kalendár' THEN 15
    WHEN N'Vyhlásenie konkurzu' THEN 16
    WHEN N'Zaplatenie splatných pohľadávok' THEN 17
    WHEN N'Žiadny veriteľ' THEN 18
    ELSE NULL END");

            migrationBuilder.Sql(@"
UPDATE KonaniaOsoby SET TypId = CASE Typ
    WHEN 'FyzickaOsoba' THEN 1
    WHEN 'FyzickaOsobaPodnikatel' THEN 2
    WHEN 'PravnickaOsoba' THEN 3
    ELSE NULL END");

            migrationBuilder.Sql(@"
UPDATE KonaniaOsoby SET RoleId = CASE Role
    WHEN 'dlznik' THEN 1
    WHEN 'navrhovatel' THEN 2
    ELSE NULL END");

            // Pozn.: UPDATE VerejneOznamy sa spúšťa samostatne (670K riadkov > timeout migrácie)
            // Spusti po migrácii: sqlcmd -S "(localdb)\mssqllocaldb" -d IRSlovensko -t 600 -i migrate_verejneoznamy.sql

            // 3. Zmazať staré string stĺpce
            migrationBuilder.DropColumn(
                name: "Typ",
                table: "Konania");

            migrationBuilder.DropColumn(
                name: "StavKonania",
                table: "Konania");

            migrationBuilder.DropColumn(
                name: "DovodUkonceniaProcesu",
                table: "Konania");

            migrationBuilder.DropColumn(
                name: "TypSpravcu",
                table: "Konania");

            migrationBuilder.DropColumn(
                name: "TypPrideleniaSpravcu",
                table: "Konania");

            migrationBuilder.DropColumn(
                name: "Typ",
                table: "KonaniaOsoby");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "KonaniaOsoby");

            migrationBuilder.DropColumn(
                name: "OznamTyp",
                table: "VerejneOznamy");

            migrationBuilder.DropColumn(
                name: "KonanieTyp",
                table: "VerejneOznamy");

            migrationBuilder.DropColumn(
                name: "DruhPodania",
                table: "VerejneOznamy");

            // 4. Indexy
            migrationBuilder.CreateIndex(
                name: "IX_Konania_TypId",
                table: "Konania",
                column: "TypId");

            migrationBuilder.CreateIndex(
                name: "IX_Konania_StavKonaniaId",
                table: "Konania",
                column: "StavKonaniaId");

            migrationBuilder.CreateIndex(
                name: "IX_Konania_DovodUkonceniaProcesuId",
                table: "Konania",
                column: "DovodUkonceniaProcesuId");

            migrationBuilder.CreateIndex(
                name: "IX_Konania_TypSpravcuId",
                table: "Konania",
                column: "TypSpravcuId");

            migrationBuilder.CreateIndex(
                name: "IX_Konania_TypPrideleniaSpravcuId",
                table: "Konania",
                column: "TypPrideleniaSpravcuId");

            migrationBuilder.CreateIndex(
                name: "IX_KonaniaOsoby_TypId",
                table: "KonaniaOsoby",
                column: "TypId");

            migrationBuilder.CreateIndex(
                name: "IX_KonaniaOsoby_RoleId",
                table: "KonaniaOsoby",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_VerejneOznamy_OznamTypId",
                table: "VerejneOznamy",
                column: "OznamTypId");

            migrationBuilder.CreateIndex(
                name: "IX_VerejneOznamy_KonanieTypId",
                table: "VerejneOznamy",
                column: "KonanieTypId");

            migrationBuilder.CreateIndex(
                name: "IX_VerejneOznamy_DruhPodaniaId",
                table: "VerejneOznamy",
                column: "DruhPodaniaId");

            // 5. Cudzie kľúče
            migrationBuilder.AddForeignKey(
                name: "FK_Konania_CSIRKonaniaTyp_TypId",
                table: "Konania",
                column: "TypId",
                principalTable: "CSIRKonaniaTyp",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Konania_CSIRKonaniaStavKonania_StavKonaniaId",
                table: "Konania",
                column: "StavKonaniaId",
                principalTable: "CSIRKonaniaStavKonania",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Konania_CSIRKonaniaDovodUkonceniaProcesu_DovodUkonceniaProcesuId",
                table: "Konania",
                column: "DovodUkonceniaProcesuId",
                principalTable: "CSIRKonaniaDovodUkonceniaProcesu",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Konania_CSIRKonaniaTypSpravcu_TypSpravcuId",
                table: "Konania",
                column: "TypSpravcuId",
                principalTable: "CSIRKonaniaTypSpravcu",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Konania_CSIRKonaniaTypPrideleniaSpravcu_TypPrideleniaSpravcuId",
                table: "Konania",
                column: "TypPrideleniaSpravcuId",
                principalTable: "CSIRKonaniaTypPrideleniaSpravcu",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_KonaniaOsoby_CSIRKonaniaOsobyTyp_TypId",
                table: "KonaniaOsoby",
                column: "TypId",
                principalTable: "CSIRKonaniaOsobyTyp",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_KonaniaOsoby_CSIRKonaniaOsobyRole_RoleId",
                table: "KonaniaOsoby",
                column: "RoleId",
                principalTable: "CSIRKonaniaOsobyRole",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VerejneOznamy_CSIRVerejneOznamyOznamTyp_OznamTypId",
                table: "VerejneOznamy",
                column: "OznamTypId",
                principalTable: "CSIRVerejneOznamyOznamTyp",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VerejneOznamy_CSIRVerejneOznamyKonanieTyp_KonanieTypId",
                table: "VerejneOznamy",
                column: "KonanieTypId",
                principalTable: "CSIRVerejneOznamyKonanieTyp",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VerejneOznamy_CSIRVerejneOznamyDruhPodania_DruhPodaniaId",
                table: "VerejneOznamy",
                column: "DruhPodaniaId",
                principalTable: "CSIRVerejneOznamyDruhPodania",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Konania_CSIRKonaniaDovodUkonceniaProcesu_DovodUkonceniaProcesuId",
                table: "Konania");

            migrationBuilder.DropForeignKey(
                name: "FK_Konania_CSIRKonaniaStavKonania_StavKonaniaId",
                table: "Konania");

            migrationBuilder.DropForeignKey(
                name: "FK_Konania_CSIRKonaniaTypPrideleniaSpravcu_TypPrideleniaSpravcuId",
                table: "Konania");

            migrationBuilder.DropForeignKey(
                name: "FK_Konania_CSIRKonaniaTypSpravcu_TypSpravcuId",
                table: "Konania");

            migrationBuilder.DropForeignKey(
                name: "FK_Konania_CSIRKonaniaTyp_TypId",
                table: "Konania");

            migrationBuilder.DropForeignKey(
                name: "FK_KonaniaOsoby_CSIRKonaniaOsobyRole_RoleId",
                table: "KonaniaOsoby");

            migrationBuilder.DropForeignKey(
                name: "FK_KonaniaOsoby_CSIRKonaniaOsobyTyp_TypId",
                table: "KonaniaOsoby");

            migrationBuilder.DropForeignKey(
                name: "FK_VerejneOznamy_CSIRVerejneOznamyDruhPodania_DruhPodaniaId",
                table: "VerejneOznamy");

            migrationBuilder.DropForeignKey(
                name: "FK_VerejneOznamy_CSIRVerejneOznamyKonanieTyp_KonanieTypId",
                table: "VerejneOznamy");

            migrationBuilder.DropForeignKey(
                name: "FK_VerejneOznamy_CSIRVerejneOznamyOznamTyp_OznamTypId",
                table: "VerejneOznamy");

            migrationBuilder.DropIndex(name: "IX_VerejneOznamy_DruhPodaniaId", table: "VerejneOznamy");
            migrationBuilder.DropIndex(name: "IX_VerejneOznamy_KonanieTypId", table: "VerejneOznamy");
            migrationBuilder.DropIndex(name: "IX_VerejneOznamy_OznamTypId", table: "VerejneOznamy");
            migrationBuilder.DropIndex(name: "IX_KonaniaOsoby_RoleId", table: "KonaniaOsoby");
            migrationBuilder.DropIndex(name: "IX_KonaniaOsoby_TypId", table: "KonaniaOsoby");
            migrationBuilder.DropIndex(name: "IX_Konania_DovodUkonceniaProcesuId", table: "Konania");
            migrationBuilder.DropIndex(name: "IX_Konania_StavKonaniaId", table: "Konania");
            migrationBuilder.DropIndex(name: "IX_Konania_TypId", table: "Konania");
            migrationBuilder.DropIndex(name: "IX_Konania_TypPrideleniaSpravcuId", table: "Konania");
            migrationBuilder.DropIndex(name: "IX_Konania_TypSpravcuId", table: "Konania");

            migrationBuilder.DropColumn(name: "DruhPodaniaId", table: "VerejneOznamy");
            migrationBuilder.DropColumn(name: "KonanieTypId", table: "VerejneOznamy");
            migrationBuilder.DropColumn(name: "OznamTypId", table: "VerejneOznamy");
            migrationBuilder.DropColumn(name: "RoleId", table: "KonaniaOsoby");
            migrationBuilder.DropColumn(name: "TypId", table: "KonaniaOsoby");
            migrationBuilder.DropColumn(name: "DovodUkonceniaProcesuId", table: "Konania");
            migrationBuilder.DropColumn(name: "StavKonaniaId", table: "Konania");
            migrationBuilder.DropColumn(name: "TypId", table: "Konania");
            migrationBuilder.DropColumn(name: "TypPrideleniaSpravcuId", table: "Konania");
            migrationBuilder.DropColumn(name: "TypSpravcuId", table: "Konania");

            migrationBuilder.AddColumn<string>(name: "DruhPodania", table: "VerejneOznamy", type: "nvarchar(max)", nullable: true);
            migrationBuilder.AddColumn<string>(name: "KonanieTyp", table: "VerejneOznamy", type: "nvarchar(max)", nullable: true);
            migrationBuilder.AddColumn<string>(name: "OznamTyp", table: "VerejneOznamy", type: "nvarchar(max)", nullable: true);
            migrationBuilder.AddColumn<string>(name: "Role", table: "KonaniaOsoby", type: "nvarchar(max)", nullable: true);
            migrationBuilder.AddColumn<string>(name: "Typ", table: "KonaniaOsoby", type: "nvarchar(max)", nullable: true);
            migrationBuilder.AddColumn<string>(name: "DovodUkonceniaProcesu", table: "Konania", type: "nvarchar(max)", nullable: true);
            migrationBuilder.AddColumn<string>(name: "StavKonania", table: "Konania", type: "nvarchar(max)", nullable: true);
            migrationBuilder.AddColumn<string>(name: "Typ", table: "Konania", type: "nvarchar(max)", nullable: true);
            migrationBuilder.AddColumn<string>(name: "TypPrideleniaSpravcu", table: "Konania", type: "nvarchar(max)", nullable: true);
            migrationBuilder.AddColumn<string>(name: "TypSpravcu", table: "Konania", type: "nvarchar(max)", nullable: true);
        }
    }
}
