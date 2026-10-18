using Microsoft.EntityFrameworkCore.Migrations;

namespace HardwareStore.Migrations
{
    /// <summary>
    /// This migration will not work because it was not generated using the following command 
    /// `dotnet ef migrations add  DeleteDataFromAllTables`
    /// </summary>
    public partial class DeleteDataFromAllTables:Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var createStatement =
                "CREATE PROCEDURE [dbo].[uspDeleteAllDataFromDatabase]\r\n" +
                "\r\nAS\r\n" +
                "BEGIN\r\n\r\n" +
                "    SET NOCOUNT ON;\n" +

                "--Deleting content of junction tables\r\n" +
                "DELETE FROM BrandsSuppliers\r\n" +
                "DELETE FROM ProductsBrands\r\n" +
                "DELETE FROM ProductsBins\r\n" +
                "DELETE FROM ProductsManufacturers\r\n" +
                "DELETE FROM ProductsSuppliers\r\n" +
                "DELETE FROM ProductsCategories\r\n" +
                "DELETE FROM ProductsCountries\r\n" +
                "DELETE FROM ProductsUnits\r\n" +
                
                "--Deleting content of non-junction child tables \r\n" +
                "DELETE FROM SubCategories\r\n" +


                "--Deleting content of parent tables\r\n" +
                "-- Deleting parent tables (tables that does not depend on any other table)\r\n" +
                
                "DELETE FROM Bins\r\n" +

                "DELETE FROM Brands\r\n" +
 
                "DELETE FROM Categories\r\n" +

                "DELETE FROM Countries\r\n" +


                "DELETE FROM Manufacturers\r\n" +

                "DELETE FROM Products\r\n" +

                "DELETE FROM Units\r\n" +

                "DELETE FROM Suppliers\r\n" +

                "END;";
            migrationBuilder.Sql(createStatement);
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var dropStatement = "DROP PROCEDURE [dbo].[uspDeleteAllDataFromDatabase]";
            migrationBuilder.Sql(dropStatement);
        }
    }
}
