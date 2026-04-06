using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IRSlovensko.Migrations
{
    /// <inheritdoc />
    public partial class AddKonanieInfoFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DatumPodania",
                table: "Konania",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DatumPoslednejUdalosti",
                table: "Konania",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DatumUkonceniaProcesu",
                table: "Konania",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DlznikDatumNarodenia",
                table: "Konania",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DlznikIco",
                table: "Konania",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DlznikMeno",
                table: "Konania",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DovodUkonceniaProcesu",
                table: "Konania",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PoslednaUdalost",
                table: "Konania",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StavKonania",
                table: "Konania",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DatumPodania",
                table: "Konania");

            migrationBuilder.DropColumn(
                name: "DatumPoslednejUdalosti",
                table: "Konania");

            migrationBuilder.DropColumn(
                name: "DatumUkonceniaProcesu",
                table: "Konania");

            migrationBuilder.DropColumn(
                name: "DlznikDatumNarodenia",
                table: "Konania");

            migrationBuilder.DropColumn(
                name: "DlznikIco",
                table: "Konania");

            migrationBuilder.DropColumn(
                name: "DlznikMeno",
                table: "Konania");

            migrationBuilder.DropColumn(
                name: "DovodUkonceniaProcesu",
                table: "Konania");

            migrationBuilder.DropColumn(
                name: "PoslednaUdalost",
                table: "Konania");

            migrationBuilder.DropColumn(
                name: "StavKonania",
                table: "Konania");
        }
    }
}
