using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectSolamnia.Migrations
{
    /// <inheritdoc />
    public partial class _180625 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Intigue",
                table: "Characters",
                newName: "Intrigue");

            migrationBuilder.AddColumn<int>(
                name: "BonusDiplomacy",
                table: "Traits",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BonusIntrigue",
                table: "Traits",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BonusLearning",
                table: "Traits",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BonusMartial",
                table: "Traits",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BonusProwess",
                table: "Traits",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BonusStewardship",
                table: "Traits",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BonusDiplomacy",
                table: "Traits");

            migrationBuilder.DropColumn(
                name: "BonusIntrigue",
                table: "Traits");

            migrationBuilder.DropColumn(
                name: "BonusLearning",
                table: "Traits");

            migrationBuilder.DropColumn(
                name: "BonusMartial",
                table: "Traits");

            migrationBuilder.DropColumn(
                name: "BonusProwess",
                table: "Traits");

            migrationBuilder.DropColumn(
                name: "BonusStewardship",
                table: "Traits");

            migrationBuilder.RenameColumn(
                name: "Intrigue",
                table: "Characters",
                newName: "Intigue");
        }
    }
}
