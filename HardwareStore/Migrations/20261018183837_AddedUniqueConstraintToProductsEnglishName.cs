using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HardwareStore.Migrations
{
    /// <inheritdoc />
    public partial class AddedUniqueConstraintToProductsEnglishName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SubCategories_CategoryId_EnglishName",
                table: "SubCategories");

            migrationBuilder.AlterColumn<string>(
                name: "EnglishName",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_CategoryId",
                table: "SubCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_EnglishName",
                table: "SubCategories",
                column: "EnglishName",
                unique: true,
                filter: "[EnglishName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Products_EnglishName",
                table: "Products",
                column: "EnglishName",
                unique: true,
                filter: "[EnglishName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SubCategories_CategoryId",
                table: "SubCategories");

            migrationBuilder.DropIndex(
                name: "IX_SubCategories_EnglishName",
                table: "SubCategories");

            migrationBuilder.DropIndex(
                name: "IX_Products_EnglishName",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "EnglishName",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_CategoryId_EnglishName",
                table: "SubCategories",
                columns: new[] { "CategoryId", "EnglishName" },
                unique: true,
                filter: "[EnglishName] IS NOT NULL");
        }
    }
}
