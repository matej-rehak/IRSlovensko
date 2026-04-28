using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IRSlovensko.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDlznikRedundantColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DlznikDatumNarodenia",
                table: "Konania");

            migrationBuilder.DropColumn(
                name: "DlznikIco",
                table: "Konania");

            migrationBuilder.DropColumn(
                name: "DlznikMeno",
                table: "Konania");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
