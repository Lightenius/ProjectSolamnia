using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectSolamnia.Migrations
{
    /// <inheritdoc />
    public partial class AddExclusiveWithTraitIdsToTrait : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExclusiveGroup",
                table: "Traits");

            migrationBuilder.AddColumn<string>(
                name: "ExclusiveTraitIds",
                table: "Traits",
                type: "TEXT",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Characters",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "Activeduty",
                table: "Characters",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Characters",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AssignedHoldingId",
                table: "Characters",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Diplomacy",
                table: "Characters",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Intigue",
                table: "Characters",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Learning",
                table: "Characters",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Martial",
                table: "Characters",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Prowess",
                table: "Characters",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Stewardship",
                table: "Characters",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Holding",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holding", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Characters_AssignedHoldingId",
                table: "Characters",
                column: "AssignedHoldingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Holding_AssignedHoldingId",
                table: "Characters",
                column: "AssignedHoldingId",
                principalTable: "Holding",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Holding_AssignedHoldingId",
                table: "Characters");

            migrationBuilder.DropTable(
                name: "Holding");

            migrationBuilder.DropIndex(
                name: "IX_Characters_AssignedHoldingId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "ExclusiveTraitIds",
                table: "Traits");

            migrationBuilder.DropColumn(
                name: "Activeduty",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "AssignedHoldingId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Diplomacy",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Intigue",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Learning",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Martial",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Prowess",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Stewardship",
                table: "Characters");

            migrationBuilder.AddColumn<string>(
                name: "ExclusiveGroup",
                table: "Traits",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Characters",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }
    }
}
