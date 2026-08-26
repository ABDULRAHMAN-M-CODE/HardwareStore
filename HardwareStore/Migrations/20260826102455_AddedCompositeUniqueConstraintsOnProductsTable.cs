using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HardwareStore.Migrations
{
    /// <inheritdoc />
    public partial class AddedCompositeUniqueConstraintsOnProductsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SubCategories_CategoryId",
                table: "SubCategories");

            migrationBuilder.DropIndex(
                name: "IX_SubCategories_EnglishName_CategoryId",
                table: "SubCategories");

            migrationBuilder.DropIndex(
                name: "IX_Products_BrandId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_UnitId",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "EnglishName",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_CategoryId_EnglishName",
                table: "SubCategories",
                columns: new[] { "CategoryId", "EnglishName" },
                unique: true,
                filter: "[EnglishName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId_EnglishName",
                table: "Products",
                columns: new[] { "BrandId", "EnglishName" },
                unique: true,
                filter: "[EnglishName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId_EnglishName",
                table: "Products",
                columns: new[] { "CategoryId", "EnglishName" },
                unique: true,
                filter: "[EnglishName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SupplierId_EnglishName",
                table: "Products",
                columns: new[] { "SupplierId", "EnglishName" },
                unique: true,
                filter: "[EnglishName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UnitId_EnglishName",
                table: "Products",
                columns: new[] { "UnitId", "EnglishName" },
                unique: true,
                filter: "[EnglishName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SubCategories_CategoryId_EnglishName",
                table: "SubCategories");

            migrationBuilder.DropIndex(
                name: "IX_Products_BrandId_EnglishName",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryId_EnglishName",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_SupplierId_EnglishName",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_UnitId_EnglishName",
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
                name: "IX_SubCategories_CategoryId",
                table: "SubCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_EnglishName_CategoryId",
                table: "SubCategories",
                columns: new[] { "EnglishName", "CategoryId" },
                unique: true,
                filter: "[EnglishName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                table: "Products",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UnitId",
                table: "Products",
                column: "UnitId");
        }
    }
}
