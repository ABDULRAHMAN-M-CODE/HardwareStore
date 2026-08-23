using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HardwareStore.Migrations
{
    /// <inheritdoc />
    public partial class AddedSupplierAndBrandNavigationPropertiesToProductsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Products_SupplierId",
                table: "Products",
                column: "SupplierId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Products_Brands_BrandId",
            //    table: "Products",
            //    column: "BrandId",
            //    principalTable: "Brands",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Products_Suppliers_SupplierId",
            //    table: "Products",
            //    column: "SupplierId",
            //    principalTable: "Suppliers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Products_Suppliers_SupplierId",
            //    table: "Products");

            //migrationBuilder.DropIndex(
            //    name: "IX_Products_SupplierId",
            //    table: "Products");
        }
    }
}
