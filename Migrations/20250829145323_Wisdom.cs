using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectSolamnia.Migrations
{
    /// <inheritdoc />
    public partial class Wisdom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActiveDuty",
                table: "Characters",
                newName: "Mission");

            migrationBuilder.AddColumn<int>(
                name: "WisDip",
                table: "Characters",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WisInt",
                table: "Characters",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WisLea",
                table: "Characters",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WisMar",
                table: "Characters",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WisPro",
                table: "Characters",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WisSte",
                table: "Characters",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WisdomBaselineDip",
                table: "Characters",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WisdomBaselineInt",
                table: "Characters",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WisdomBaselineLea",
                table: "Characters",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WisdomBaselineMar",
                table: "Characters",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WisdomBaselinePro",
                table: "Characters",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WisdomBaselineSte",
                table: "Characters",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WisdomPointsApplied",
                table: "Characters",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WisdomSeed",
                table: "Characters",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WisDip",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "WisInt",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "WisLea",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "WisMar",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "WisPro",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "WisSte",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "WisdomBaselineDip",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "WisdomBaselineInt",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "WisdomBaselineLea",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "WisdomBaselineMar",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "WisdomBaselinePro",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "WisdomBaselineSte",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "WisdomPointsApplied",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "WisdomSeed",
                table: "Characters");

            migrationBuilder.RenameColumn(
                name: "Mission",
                table: "Characters",
                newName: "ActiveDuty");
        }
    }
}
