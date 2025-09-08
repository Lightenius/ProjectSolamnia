using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectSolamnia.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Stewardship",
                table: "Characters",
                newName: "BaseStewardship");

            migrationBuilder.RenameColumn(
                name: "Prowess",
                table: "Characters",
                newName: "BaseProwess");

            migrationBuilder.RenameColumn(
                name: "Martial",
                table: "Characters",
                newName: "BaseMartial");

            migrationBuilder.RenameColumn(
                name: "Learning",
                table: "Characters",
                newName: "BaseLearning");

            migrationBuilder.RenameColumn(
                name: "Intrigue",
                table: "Characters",
                newName: "BaseIntrigue");

            migrationBuilder.RenameColumn(
                name: "Diplomacy",
                table: "Characters",
                newName: "BaseDiplomacy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BaseStewardship",
                table: "Characters",
                newName: "Stewardship");

            migrationBuilder.RenameColumn(
                name: "BaseProwess",
                table: "Characters",
                newName: "Prowess");

            migrationBuilder.RenameColumn(
                name: "BaseMartial",
                table: "Characters",
                newName: "Martial");

            migrationBuilder.RenameColumn(
                name: "BaseLearning",
                table: "Characters",
                newName: "Learning");

            migrationBuilder.RenameColumn(
                name: "BaseIntrigue",
                table: "Characters",
                newName: "Intrigue");

            migrationBuilder.RenameColumn(
                name: "BaseDiplomacy",
                table: "Characters",
                newName: "Diplomacy");
        }
    }
}
