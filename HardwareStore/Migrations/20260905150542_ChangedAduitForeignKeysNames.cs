using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HardwareStore.Migrations
{
    /// <inheritdoc />
    public partial class ChangedAduitForeignKeysNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bins_AspNetUsers_CreatedBy",
                table: "Bins");

            migrationBuilder.DropForeignKey(
                name: "FK_Bins_AspNetUsers_DeletedBy",
                table: "Bins");

            migrationBuilder.DropForeignKey(
                name: "FK_Bins_AspNetUsers_UpdatedBy",
                table: "Bins");

            migrationBuilder.DropForeignKey(
                name: "FK_Brands_AspNetUsers_CreatedBy",
                table: "Brands");

            migrationBuilder.DropForeignKey(
                name: "FK_Brands_AspNetUsers_DeletedBy",
                table: "Brands");

            migrationBuilder.DropForeignKey(
                name: "FK_Brands_AspNetUsers_UpdatedBy",
                table: "Brands");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AspNetUsers_CreatedBy",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AspNetUsers_DeletedBy",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AspNetUsers_UpdatedBy",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Countries_AspNetUsers_CreatedBy",
                table: "Countries");

            migrationBuilder.DropForeignKey(
                name: "FK_Countries_AspNetUsers_DeletedBy",
                table: "Countries");

            migrationBuilder.DropForeignKey(
                name: "FK_Countries_AspNetUsers_UpdatedBy",
                table: "Countries");

            migrationBuilder.DropForeignKey(
                name: "FK_Manufacturers_AspNetUsers_CreatedBy",
                table: "Manufacturers");

            migrationBuilder.DropForeignKey(
                name: "FK_Manufacturers_AspNetUsers_DeletedBy",
                table: "Manufacturers");

            migrationBuilder.DropForeignKey(
                name: "FK_Manufacturers_AspNetUsers_UpdatedBy",
                table: "Manufacturers");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_CreatedBy",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_DeletedBy",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_UpdatedBy",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_SubCategories_AspNetUsers_CreatedBy",
                table: "SubCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_SubCategories_AspNetUsers_DeletedBy",
                table: "SubCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_SubCategories_AspNetUsers_UpdatedBy",
                table: "SubCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_AspNetUsers_CreatedBy",
                table: "Suppliers");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_AspNetUsers_DeletedBy",
                table: "Suppliers");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_AspNetUsers_UpdatedBy",
                table: "Suppliers");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_AspNetUsers_CreatedBy",
                table: "Units");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_AspNetUsers_DeletedBy",
                table: "Units");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_AspNetUsers_UpdatedBy",
                table: "Units");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Units",
                newName: "UpdaterId");

            migrationBuilder.RenameColumn(
                name: "DeletedBy",
                table: "Units",
                newName: "DeleterId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Units",
                newName: "CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Units_UpdatedBy",
                table: "Units",
                newName: "IX_Units_UpdaterId");

            migrationBuilder.RenameIndex(
                name: "IX_Units_DeletedBy",
                table: "Units",
                newName: "IX_Units_DeleterId");

            migrationBuilder.RenameIndex(
                name: "IX_Units_CreatedBy",
                table: "Units",
                newName: "IX_Units_CreatorId");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Suppliers",
                newName: "UpdaterId");

            migrationBuilder.RenameColumn(
                name: "DeletedBy",
                table: "Suppliers",
                newName: "DeleterId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Suppliers",
                newName: "CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Suppliers_UpdatedBy",
                table: "Suppliers",
                newName: "IX_Suppliers_UpdaterId");

            migrationBuilder.RenameIndex(
                name: "IX_Suppliers_DeletedBy",
                table: "Suppliers",
                newName: "IX_Suppliers_DeleterId");

            migrationBuilder.RenameIndex(
                name: "IX_Suppliers_CreatedBy",
                table: "Suppliers",
                newName: "IX_Suppliers_CreatorId");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "SubCategories",
                newName: "UpdaterId");

            migrationBuilder.RenameColumn(
                name: "DeletedBy",
                table: "SubCategories",
                newName: "DeleterId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "SubCategories",
                newName: "CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_SubCategories_UpdatedBy",
                table: "SubCategories",
                newName: "IX_SubCategories_UpdaterId");

            migrationBuilder.RenameIndex(
                name: "IX_SubCategories_DeletedBy",
                table: "SubCategories",
                newName: "IX_SubCategories_DeleterId");

            migrationBuilder.RenameIndex(
                name: "IX_SubCategories_CreatedBy",
                table: "SubCategories",
                newName: "IX_SubCategories_CreatorId");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Products",
                newName: "UpdaterId");

            migrationBuilder.RenameColumn(
                name: "DeletedBy",
                table: "Products",
                newName: "DeleterId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Products",
                newName: "CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_UpdatedBy",
                table: "Products",
                newName: "IX_Products_UpdaterId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_DeletedBy",
                table: "Products",
                newName: "IX_Products_DeleterId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CreatedBy",
                table: "Products",
                newName: "IX_Products_CreatorId");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Manufacturers",
                newName: "UpdaterId");

            migrationBuilder.RenameColumn(
                name: "DeletedBy",
                table: "Manufacturers",
                newName: "DeleterId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Manufacturers",
                newName: "CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Manufacturers_UpdatedBy",
                table: "Manufacturers",
                newName: "IX_Manufacturers_UpdaterId");

            migrationBuilder.RenameIndex(
                name: "IX_Manufacturers_DeletedBy",
                table: "Manufacturers",
                newName: "IX_Manufacturers_DeleterId");

            migrationBuilder.RenameIndex(
                name: "IX_Manufacturers_CreatedBy",
                table: "Manufacturers",
                newName: "IX_Manufacturers_CreatorId");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Countries",
                newName: "UpdaterId");

            migrationBuilder.RenameColumn(
                name: "DeletedBy",
                table: "Countries",
                newName: "DeleterId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Countries",
                newName: "CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Countries_UpdatedBy",
                table: "Countries",
                newName: "IX_Countries_UpdaterId");

            migrationBuilder.RenameIndex(
                name: "IX_Countries_DeletedBy",
                table: "Countries",
                newName: "IX_Countries_DeleterId");

            migrationBuilder.RenameIndex(
                name: "IX_Countries_CreatedBy",
                table: "Countries",
                newName: "IX_Countries_CreatorId");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Categories",
                newName: "UpdaterId");

            migrationBuilder.RenameColumn(
                name: "DeletedBy",
                table: "Categories",
                newName: "DeleterId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Categories",
                newName: "CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_UpdatedBy",
                table: "Categories",
                newName: "IX_Categories_UpdaterId");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_DeletedBy",
                table: "Categories",
                newName: "IX_Categories_DeleterId");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_CreatedBy",
                table: "Categories",
                newName: "IX_Categories_CreatorId");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Brands",
                newName: "UpdaterId");

            migrationBuilder.RenameColumn(
                name: "DeletedBy",
                table: "Brands",
                newName: "DeleterId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Brands",
                newName: "CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Brands_UpdatedBy",
                table: "Brands",
                newName: "IX_Brands_UpdaterId");

            migrationBuilder.RenameIndex(
                name: "IX_Brands_DeletedBy",
                table: "Brands",
                newName: "IX_Brands_DeleterId");

            migrationBuilder.RenameIndex(
                name: "IX_Brands_CreatedBy",
                table: "Brands",
                newName: "IX_Brands_CreatorId");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Bins",
                newName: "UpdaterId");

            migrationBuilder.RenameColumn(
                name: "DeletedBy",
                table: "Bins",
                newName: "DeleterId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Bins",
                newName: "CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Bins_UpdatedBy",
                table: "Bins",
                newName: "IX_Bins_UpdaterId");

            migrationBuilder.RenameIndex(
                name: "IX_Bins_DeletedBy",
                table: "Bins",
                newName: "IX_Bins_DeleterId");

            migrationBuilder.RenameIndex(
                name: "IX_Bins_CreatedBy",
                table: "Bins",
                newName: "IX_Bins_CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bins_AspNetUsers_CreatorId",
                table: "Bins",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bins_AspNetUsers_DeleterId",
                table: "Bins",
                column: "DeleterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bins_AspNetUsers_UpdaterId",
                table: "Bins",
                column: "UpdaterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_AspNetUsers_CreatorId",
                table: "Brands",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_AspNetUsers_DeleterId",
                table: "Brands",
                column: "DeleterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_AspNetUsers_UpdaterId",
                table: "Brands",
                column: "UpdaterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AspNetUsers_CreatorId",
                table: "Categories",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AspNetUsers_DeleterId",
                table: "Categories",
                column: "DeleterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AspNetUsers_UpdaterId",
                table: "Categories",
                column: "UpdaterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_AspNetUsers_CreatorId",
                table: "Countries",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_AspNetUsers_DeleterId",
                table: "Countries",
                column: "DeleterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_AspNetUsers_UpdaterId",
                table: "Countries",
                column: "UpdaterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Manufacturers_AspNetUsers_CreatorId",
                table: "Manufacturers",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Manufacturers_AspNetUsers_DeleterId",
                table: "Manufacturers",
                column: "DeleterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Manufacturers_AspNetUsers_UpdaterId",
                table: "Manufacturers",
                column: "UpdaterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_CreatorId",
                table: "Products",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_DeleterId",
                table: "Products",
                column: "DeleterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_UpdaterId",
                table: "Products",
                column: "UpdaterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategories_AspNetUsers_CreatorId",
                table: "SubCategories",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategories_AspNetUsers_DeleterId",
                table: "SubCategories",
                column: "DeleterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategories_AspNetUsers_UpdaterId",
                table: "SubCategories",
                column: "UpdaterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_AspNetUsers_CreatorId",
                table: "Suppliers",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_AspNetUsers_DeleterId",
                table: "Suppliers",
                column: "DeleterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_AspNetUsers_UpdaterId",
                table: "Suppliers",
                column: "UpdaterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_AspNetUsers_CreatorId",
                table: "Units",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_AspNetUsers_DeleterId",
                table: "Units",
                column: "DeleterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_AspNetUsers_UpdaterId",
                table: "Units",
                column: "UpdaterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bins_AspNetUsers_CreatorId",
                table: "Bins");

            migrationBuilder.DropForeignKey(
                name: "FK_Bins_AspNetUsers_DeleterId",
                table: "Bins");

            migrationBuilder.DropForeignKey(
                name: "FK_Bins_AspNetUsers_UpdaterId",
                table: "Bins");

            migrationBuilder.DropForeignKey(
                name: "FK_Brands_AspNetUsers_CreatorId",
                table: "Brands");

            migrationBuilder.DropForeignKey(
                name: "FK_Brands_AspNetUsers_DeleterId",
                table: "Brands");

            migrationBuilder.DropForeignKey(
                name: "FK_Brands_AspNetUsers_UpdaterId",
                table: "Brands");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AspNetUsers_CreatorId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AspNetUsers_DeleterId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AspNetUsers_UpdaterId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Countries_AspNetUsers_CreatorId",
                table: "Countries");

            migrationBuilder.DropForeignKey(
                name: "FK_Countries_AspNetUsers_DeleterId",
                table: "Countries");

            migrationBuilder.DropForeignKey(
                name: "FK_Countries_AspNetUsers_UpdaterId",
                table: "Countries");

            migrationBuilder.DropForeignKey(
                name: "FK_Manufacturers_AspNetUsers_CreatorId",
                table: "Manufacturers");

            migrationBuilder.DropForeignKey(
                name: "FK_Manufacturers_AspNetUsers_DeleterId",
                table: "Manufacturers");

            migrationBuilder.DropForeignKey(
                name: "FK_Manufacturers_AspNetUsers_UpdaterId",
                table: "Manufacturers");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_CreatorId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_DeleterId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_UpdaterId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_SubCategories_AspNetUsers_CreatorId",
                table: "SubCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_SubCategories_AspNetUsers_DeleterId",
                table: "SubCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_SubCategories_AspNetUsers_UpdaterId",
                table: "SubCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_AspNetUsers_CreatorId",
                table: "Suppliers");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_AspNetUsers_DeleterId",
                table: "Suppliers");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_AspNetUsers_UpdaterId",
                table: "Suppliers");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_AspNetUsers_CreatorId",
                table: "Units");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_AspNetUsers_DeleterId",
                table: "Units");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_AspNetUsers_UpdaterId",
                table: "Units");

            migrationBuilder.RenameColumn(
                name: "UpdaterId",
                table: "Units",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "DeleterId",
                table: "Units",
                newName: "DeletedBy");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Units",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Units_UpdaterId",
                table: "Units",
                newName: "IX_Units_UpdatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Units_DeleterId",
                table: "Units",
                newName: "IX_Units_DeletedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Units_CreatorId",
                table: "Units",
                newName: "IX_Units_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdaterId",
                table: "Suppliers",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "DeleterId",
                table: "Suppliers",
                newName: "DeletedBy");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Suppliers",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Suppliers_UpdaterId",
                table: "Suppliers",
                newName: "IX_Suppliers_UpdatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Suppliers_DeleterId",
                table: "Suppliers",
                newName: "IX_Suppliers_DeletedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Suppliers_CreatorId",
                table: "Suppliers",
                newName: "IX_Suppliers_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdaterId",
                table: "SubCategories",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "DeleterId",
                table: "SubCategories",
                newName: "DeletedBy");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "SubCategories",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_SubCategories_UpdaterId",
                table: "SubCategories",
                newName: "IX_SubCategories_UpdatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_SubCategories_DeleterId",
                table: "SubCategories",
                newName: "IX_SubCategories_DeletedBy");

            migrationBuilder.RenameIndex(
                name: "IX_SubCategories_CreatorId",
                table: "SubCategories",
                newName: "IX_SubCategories_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdaterId",
                table: "Products",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "DeleterId",
                table: "Products",
                newName: "DeletedBy");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Products",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Products_UpdaterId",
                table: "Products",
                newName: "IX_Products_UpdatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Products_DeleterId",
                table: "Products",
                newName: "IX_Products_DeletedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CreatorId",
                table: "Products",
                newName: "IX_Products_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdaterId",
                table: "Manufacturers",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "DeleterId",
                table: "Manufacturers",
                newName: "DeletedBy");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Manufacturers",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Manufacturers_UpdaterId",
                table: "Manufacturers",
                newName: "IX_Manufacturers_UpdatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Manufacturers_DeleterId",
                table: "Manufacturers",
                newName: "IX_Manufacturers_DeletedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Manufacturers_CreatorId",
                table: "Manufacturers",
                newName: "IX_Manufacturers_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdaterId",
                table: "Countries",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "DeleterId",
                table: "Countries",
                newName: "DeletedBy");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Countries",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Countries_UpdaterId",
                table: "Countries",
                newName: "IX_Countries_UpdatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Countries_DeleterId",
                table: "Countries",
                newName: "IX_Countries_DeletedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Countries_CreatorId",
                table: "Countries",
                newName: "IX_Countries_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdaterId",
                table: "Categories",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "DeleterId",
                table: "Categories",
                newName: "DeletedBy");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Categories",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_UpdaterId",
                table: "Categories",
                newName: "IX_Categories_UpdatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_DeleterId",
                table: "Categories",
                newName: "IX_Categories_DeletedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_CreatorId",
                table: "Categories",
                newName: "IX_Categories_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdaterId",
                table: "Brands",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "DeleterId",
                table: "Brands",
                newName: "DeletedBy");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Brands",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Brands_UpdaterId",
                table: "Brands",
                newName: "IX_Brands_UpdatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Brands_DeleterId",
                table: "Brands",
                newName: "IX_Brands_DeletedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Brands_CreatorId",
                table: "Brands",
                newName: "IX_Brands_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdaterId",
                table: "Bins",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "DeleterId",
                table: "Bins",
                newName: "DeletedBy");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Bins",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Bins_UpdaterId",
                table: "Bins",
                newName: "IX_Bins_UpdatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Bins_DeleterId",
                table: "Bins",
                newName: "IX_Bins_DeletedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Bins_CreatorId",
                table: "Bins",
                newName: "IX_Bins_CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Bins_AspNetUsers_CreatedBy",
                table: "Bins",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bins_AspNetUsers_DeletedBy",
                table: "Bins",
                column: "DeletedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bins_AspNetUsers_UpdatedBy",
                table: "Bins",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_AspNetUsers_CreatedBy",
                table: "Brands",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_AspNetUsers_DeletedBy",
                table: "Brands",
                column: "DeletedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_AspNetUsers_UpdatedBy",
                table: "Brands",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AspNetUsers_CreatedBy",
                table: "Categories",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AspNetUsers_DeletedBy",
                table: "Categories",
                column: "DeletedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AspNetUsers_UpdatedBy",
                table: "Categories",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_AspNetUsers_CreatedBy",
                table: "Countries",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_AspNetUsers_DeletedBy",
                table: "Countries",
                column: "DeletedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_AspNetUsers_UpdatedBy",
                table: "Countries",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Manufacturers_AspNetUsers_CreatedBy",
                table: "Manufacturers",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Manufacturers_AspNetUsers_DeletedBy",
                table: "Manufacturers",
                column: "DeletedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Manufacturers_AspNetUsers_UpdatedBy",
                table: "Manufacturers",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_CreatedBy",
                table: "Products",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_DeletedBy",
                table: "Products",
                column: "DeletedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_UpdatedBy",
                table: "Products",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategories_AspNetUsers_CreatedBy",
                table: "SubCategories",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategories_AspNetUsers_DeletedBy",
                table: "SubCategories",
                column: "DeletedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategories_AspNetUsers_UpdatedBy",
                table: "SubCategories",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_AspNetUsers_CreatedBy",
                table: "Suppliers",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_AspNetUsers_DeletedBy",
                table: "Suppliers",
                column: "DeletedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_AspNetUsers_UpdatedBy",
                table: "Suppliers",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_AspNetUsers_CreatedBy",
                table: "Units",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_AspNetUsers_DeletedBy",
                table: "Units",
                column: "DeletedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_AspNetUsers_UpdatedBy",
                table: "Units",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
