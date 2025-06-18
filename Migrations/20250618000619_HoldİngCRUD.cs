using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectSolamnia.Migrations
{
    /// <inheritdoc />
    public partial class HoldİngCRUD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Supplies",
                table: "Holdings");

            migrationBuilder.AddColumn<int>(
                name: "SupplyLevel",
                table: "Holdings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupplyLevel",
                table: "Holdings");

            migrationBuilder.AddColumn<string>(
                name: "Supplies",
                table: "Holdings",
                type: "TEXT",
                nullable: true);
        }
    }
}
