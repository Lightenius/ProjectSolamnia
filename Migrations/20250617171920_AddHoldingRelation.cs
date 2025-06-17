using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectSolamnia.Migrations
{
    /// <inheritdoc />
    public partial class AddHoldingRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Holding_AssignedHoldingId",
                table: "Characters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Holding",
                table: "Holding");

            migrationBuilder.RenameTable(
                name: "Holding",
                newName: "Holdings");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Holdings",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Holdings",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Supplies",
                table: "Holdings",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TroopsCount",
                table: "Holdings",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Holdings",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Holdings",
                table: "Holdings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Holdings_AssignedHoldingId",
                table: "Characters",
                column: "AssignedHoldingId",
                principalTable: "Holdings",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Holdings_AssignedHoldingId",
                table: "Characters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Holdings",
                table: "Holdings");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Holdings");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "Holdings");

            migrationBuilder.DropColumn(
                name: "Supplies",
                table: "Holdings");

            migrationBuilder.DropColumn(
                name: "TroopsCount",
                table: "Holdings");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Holdings");

            migrationBuilder.RenameTable(
                name: "Holdings",
                newName: "Holding");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Holding",
                table: "Holding",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Holding_AssignedHoldingId",
                table: "Characters",
                column: "AssignedHoldingId",
                principalTable: "Holding",
                principalColumn: "Id");
        }
    }
}
