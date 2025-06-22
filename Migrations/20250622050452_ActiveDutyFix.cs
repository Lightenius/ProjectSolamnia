using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectSolamnia.Migrations
{
    /// <inheritdoc />
    public partial class ActiveDutyFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Activeduty",
                table: "Characters",
                newName: "ActiveDuty");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActiveDuty",
                table: "Characters",
                newName: "Activeduty");
        }
    }
}
