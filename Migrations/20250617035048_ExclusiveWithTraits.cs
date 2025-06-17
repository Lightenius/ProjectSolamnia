using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectSolamnia.Migrations
{
    /// <inheritdoc />
    public partial class ExclusiveWithTraits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TraitExclusives",
                columns: table => new
                {
                    TraitId = table.Column<int>(type: "INTEGER", nullable: false),
                    ExclusiveWithTraitId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TraitExclusives", x => new { x.TraitId, x.ExclusiveWithTraitId });
                    table.ForeignKey(
                        name: "FK_TraitExclusives_Traits_ExclusiveWithTraitId",
                        column: x => x.ExclusiveWithTraitId,
                        principalTable: "Traits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TraitExclusives_Traits_TraitId",
                        column: x => x.TraitId,
                        principalTable: "Traits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TraitExclusives_ExclusiveWithTraitId",
                table: "TraitExclusives",
                column: "ExclusiveWithTraitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TraitExclusives");
        }
    }
}
