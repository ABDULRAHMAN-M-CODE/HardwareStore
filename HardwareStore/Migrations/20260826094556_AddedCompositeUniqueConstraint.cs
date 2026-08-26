using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HardwareStore.Migrations
{
    /// <inheritdoc />
    public partial class AddedCompositeUniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SubCategories_EnglishName",
                table: "SubCategories");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_EnglishName_CategoryId",
                table: "SubCategories",
                columns: new[] { "EnglishName", "CategoryId" },
                unique: true,
                filter: "[EnglishName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SubCategories_EnglishName_CategoryId",
                table: "SubCategories");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_EnglishName",
                table: "SubCategories",
                column: "EnglishName",
                unique: true,
                filter: "[EnglishName] IS NOT NULL");
        }
    }
}
