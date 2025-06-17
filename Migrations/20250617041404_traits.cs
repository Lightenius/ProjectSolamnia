using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectSolamnia.Migrations
{
    /// <inheritdoc />
    public partial class traits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharacterTraits_Traits_TraitId",
                table: "CharacterTraits");

            migrationBuilder.DropColumn(
                name: "ExclusiveTraitIds",
                table: "Traits");

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterTraits_Traits_TraitId",
                table: "CharacterTraits",
                column: "TraitId",
                principalTable: "Traits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharacterTraits_Traits_TraitId",
                table: "CharacterTraits");

            migrationBuilder.AddColumn<string>(
                name: "ExclusiveTraitIds",
                table: "Traits",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterTraits_Traits_TraitId",
                table: "CharacterTraits",
                column: "TraitId",
                principalTable: "Traits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
