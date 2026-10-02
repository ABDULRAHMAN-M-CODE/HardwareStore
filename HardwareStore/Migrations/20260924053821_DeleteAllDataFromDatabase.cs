using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HardwareStore.Migrations
{
    /// <inheritdoc />
    public partial class DeleteAllDataFromDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            var sqlStatement =
                "CREATE PROCEDURE uspDeleteAllDataFromDatabase\n" +
                "AS\n" +
                "BEGIN\n" +
                "DELETE FROM BrandsSuppliers;\n" +
                "DELETE FROM ProductsBrands;\n" +
                "DELETE FROM ProductsBins;\n" +
                "DELETE FROM ProductsManufacturers;\n" +
                "DELETE FROM ProductsSuppliers\n;" +
                "DELETE FROM ProductsCategories;\n" +
                "DELETE FROM ProductsCountries;\n" +
                "DELETE FROM ProductsUnits;\n" +
                "DELETE FROM SubCategories\n" +
                "DBCC CHECKIDENT ('SubCategories', RESEED, 0);\n" +
                "-- Deleting parent tables (tables that does not depend on any other table)\n" +
                "DELETE FROM Bins\n" +
                "DBCC CHECKIDENT ('Bins', RESEED, 0);\n" +
                "DELETE FROM Brands\n" +
                "DBCC CHECKIDENT ('Brands', RESEED, 0);\n" +
                "DELETE FROM Categories\n" +
                "DBCC CHECKIDENT ('Categories', RESEED, 0);\n" +
                "DELETE FROM Countries\n" +
                "DBCC CHECKIDENT ('Countries', RESEED, 0);\n" +
                "DELETE FROM Manufacturers\r\nDBCC CHECKIDENT ('Manufacturers', RESEED, 0);\n" +
                "DELETE FROM Products\n" +
                "DBCC CHECKIDENT ('Products', RESEED, 0);\n" +
                "DELETE FROM Units\n" +
                "DBCC CHECKIDENT ('Units', RESEED, 0);\n" +
                "DELETE FROM Suppliers\n" +
                "DBCC CHECKIDENT ('Suppliers', RESEED, 0);\n" +
                "END;";

            migrationBuilder.Sql(sqlStatement);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sqlStatement= "DROP PROCEDURE [dbo].[uspDeleteAllDataFromDatabase]";
            migrationBuilder.Sql(sqlStatement);
        }
    }
}
