using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IRSlovensko.Migrations
{
    /// <inheritdoc />
    public partial class AddVerejneOznamyDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DruhPodania",
                table: "VerejneOznamy",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ObsahujePrilohy",
                table: "VerejneOznamy",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpisovaZnackaSpravcovskehoSpisu",
                table: "VerejneOznamy",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "VerejneOznamy",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TextDruh",
                table: "VerejneOznamy",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TextHlavicka",
                table: "VerejneOznamy",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TextOdovodnenie",
                table: "VerejneOznamy",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TextOznam",
                table: "VerejneOznamy",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TextPoucenie",
                table: "VerejneOznamy",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TextRozhodnutie",
                table: "VerejneOznamy",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DruhPodania",
                table: "VerejneOznamy");

            migrationBuilder.DropColumn(
                name: "ObsahujePrilohy",
                table: "VerejneOznamy");

            migrationBuilder.DropColumn(
                name: "SpisovaZnackaSpravcovskehoSpisu",
                table: "VerejneOznamy");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "VerejneOznamy");

            migrationBuilder.DropColumn(
                name: "TextDruh",
                table: "VerejneOznamy");

            migrationBuilder.DropColumn(
                name: "TextHlavicka",
                table: "VerejneOznamy");

            migrationBuilder.DropColumn(
                name: "TextOdovodnenie",
                table: "VerejneOznamy");

            migrationBuilder.DropColumn(
                name: "TextOznam",
                table: "VerejneOznamy");

            migrationBuilder.DropColumn(
                name: "TextPoucenie",
                table: "VerejneOznamy");

            migrationBuilder.DropColumn(
                name: "TextRozhodnutie",
                table: "VerejneOznamy");
        }
    }
}
