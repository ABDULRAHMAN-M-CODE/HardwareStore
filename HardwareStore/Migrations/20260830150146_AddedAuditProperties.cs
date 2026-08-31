using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HardwareStore.Migrations
{
    /// <inheritdoc />
    public partial class AddedAuditProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeleatedAt",
                table: "Units",
                newName: "DeletedAt");

            migrationBuilder.RenameColumn(
                name: "DeleatedAt",
                table: "Suppliers",
                newName: "DeletedAt");

            migrationBuilder.RenameColumn(
                name: "DeleatedAt",
                table: "SubCategories",
                newName: "DeletedAt");

            migrationBuilder.RenameColumn(
                name: "DeleatedAt",
                table: "Products",
                newName: "DeletedAt");

            migrationBuilder.RenameColumn(
                name: "DeleatedAt",
                table: "Countries",
                newName: "DeletedAt");

            migrationBuilder.RenameColumn(
                name: "DeleatedAt",
                table: "Categories",
                newName: "DeletedAt");

            migrationBuilder.RenameColumn(
                name: "DeleatedAt",
                table: "Brands",
                newName: "DeletedAt");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Units",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Units",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Units",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Suppliers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "SubCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "SubCategories",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "SubCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Countries",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Categories",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Brands",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Brands",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Brands",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Units_DeletedBy",
                table: "Units",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_DeletedBy",
                table: "Suppliers",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_DeletedBy",
                table: "SubCategories",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Products_DeletedBy",
                table: "Products",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_DeletedBy",
                table: "Countries",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_DeletedBy",
                table: "Categories",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Brands_DeletedBy",
                table: "Brands",
                column: "DeletedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_AspNetUsers_DeletedBy",
                table: "Brands",
                column: "DeletedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AspNetUsers_DeletedBy",
                table: "Categories",
                column: "DeletedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_AspNetUsers_DeletedBy",
                table: "Countries",
                column: "DeletedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_DeletedBy",
                table: "Products",
                column: "DeletedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategories_AspNetUsers_DeletedBy",
                table: "SubCategories",
                column: "DeletedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_AspNetUsers_DeletedBy",
                table: "Suppliers",
                column: "DeletedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_AspNetUsers_DeletedBy",
                table: "Units",
                column: "DeletedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brands_AspNetUsers_DeletedBy",
                table: "Brands");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AspNetUsers_DeletedBy",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Countries_AspNetUsers_DeletedBy",
                table: "Countries");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_DeletedBy",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_SubCategories_AspNetUsers_DeletedBy",
                table: "SubCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_AspNetUsers_DeletedBy",
                table: "Suppliers");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_AspNetUsers_DeletedBy",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Units_DeletedBy",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_DeletedBy",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_SubCategories_DeletedBy",
                table: "SubCategories");

            migrationBuilder.DropIndex(
                name: "IX_Products_DeletedBy",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Countries_DeletedBy",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Categories_DeletedBy",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Brands_DeletedBy",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "SubCategories");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "SubCategories");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "SubCategories");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Brands");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "Units",
                newName: "DeleatedAt");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "Suppliers",
                newName: "DeleatedAt");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "SubCategories",
                newName: "DeleatedAt");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "Products",
                newName: "DeleatedAt");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "Countries",
                newName: "DeleatedAt");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "Categories",
                newName: "DeleatedAt");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "Brands",
                newName: "DeleatedAt");
        }
    }
}
