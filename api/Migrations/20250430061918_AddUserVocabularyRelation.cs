using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AddUserVocabularyRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Vocabularies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Vocabularies_UserId",
                table: "Vocabularies",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vocabularies_Users_UserId",
                table: "Vocabularies",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vocabularies_Users_UserId",
                table: "Vocabularies");

            migrationBuilder.DropIndex(
                name: "IX_Vocabularies_UserId",
                table: "Vocabularies");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Vocabularies");
        }
    }
}
