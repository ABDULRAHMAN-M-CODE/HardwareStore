using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HardwareStore.Migrations
{
    /// <inheritdoc />
    public partial class AddedOverwriteDatabaseDataStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var createStatement =
                "USE [HardwareDB]\r\n" +
                "GO\n" +
                "DROP PROCEDURE IF EXISTS [dbo].[uspDeleteAllDataFromDatabase]\n" +
                "GO\n" +
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

                "END;\n" +
                
 
                "GO\r\n" +
                "CREATE OR ALTER PROCEDURE uspTransferDataFromSpecificColumnInStagingTableToEnglishNameColumnInTarget\r\n " +
                "@ParentTableName nvarchar(50),\r\n   " +
                " @ColumnNameInStagingTable nvarchar(50),\r\n   " +
                " @actorId nvarchar(50)\r\n" +
                "AS   \r\n" +
                "BEGIN\r\n" +
                "\r\n" +
                "    DECLARE @InsertString AS NVARCHAR (500);\r\n" +
                "    Set @InsertString='\r\n" +
                "    INSERT INTO '+QUOTENAME(@ParentTableName)+' (Id,CreatorId,UpdaterId,EnglishName,UpdatedAt,CreatedAt,DeletedAt,DeleterId)\r\n" +
                "    SELECT \r\n" +
                "        ROW_NUMBER() OVER (ORDER BY '+QUOTENAME(@ColumnNameInStagingTable)+')\r\n" +
                "            + (SELECT ISNULL(MAX(Id), 0) FROM '+QUOTENAME(@ParentTableName)+' ),\r\n" +
                "        @actorId,\r\n" +
                "        @actorId,\r\n" +
                "        '+QUOTENAME(@ColumnNameInStagingTable)+',\r\n" +
                "        SYSDATETIME(),\r\n" +
                "        SYSDATETIME(),\r\n" +
                "        ''0001-01-01 00:00:00'',\r\n" +
                "        NULL\r\n" +
                "    FROM (SELECT DISTINCT '+QUOTENAME(@ColumnNameInStagingTable)+' FROM BulkInsertedProducts) AS E;\r\n" +
                "    \r\n" +
                "    ' ;\r\n" +
                "\r\n" +
                "EXECUTE sp_executesql\r\n" +
                "    @InsertString, N'@actorId nvarchar(50)',\r\n" +
                "    @actorId;\r\n" +
                "\r\n" +
                "END\r\n" +
                "\r\n" +
                "GO\r\n" +
                "\r\n" +
                
                "DROP PROCEDURE IF EXISTS TransferDataFromStagingTableToJunctionTable;\r\n" +
                "GO\r\n" +
                "CREATE PROCEDURE TransferDataFromStagingTableToJunctionTable\r\n" +
                "    @JunctionTableName nvarchar(50),\r\n" +
                "\t@FirstColumnNameInjunctionTable nvarchar(50),\r\n" +
                "\t@SecondColumnNameInjunctionTable nvarchar(50),\r\n" +
                "\t@FirstParentTableName nvarchar(50),\r\n" +
                "\t@SecondParentTableName nvarchar(50),\r\n" +
                "\t@FirstColumnNameInStaging nvarchar(50),\r\n" +
                "\t@SecondColumnNameInStaging nvarchar(50)\r\n" +
                "AS \r\nBEGIN\r\nDECLARE @InsertString AS NVARCHAR (500);\r\n" +
                "\r\n" +
                "SET @InsertString = \r\n" +
                "\t\t\t\t\t\t'\r\n" +
                "\t\t\t\t\t\t\tSET NOCOUNT ON;\r\n" +
                "\t\t\t\t\t\t\tINSERT INTO '+QUOTENAME(@JunctionTableName)+'('+QUOTENAME(@FirstColumnNameInjunctionTable)+', '+QUOTENAME(@SecondColumnNameInjunctionTable)+')\r\n" +
                "\t\t\t\t\t\t\tSELECT DISTINCT p1.Id as '+QUOTENAME(@FirstColumnNameInjunctionTable)+',p2.Id as '+QUOTENAME(@SecondColumnNameInjunctionTable)+'\r\n" +
                "\t\t\t\t\t\t\tFROM BulkInsertedProducts bis\r\n" +
                "\t\t\t\t\t\t\tINNER JOIN '+QUOTENAME(@FirstParentTableName)+' p1 ON p1.EnglishName=bis.'+QUOTENAME(@FirstColumnNameInStaging)+'\r\n" +
                "\t\t\t\t\t\t\tINNER JOIN '+QUOTENAME(@SecondParentTableName)+' p2 on p2.EnglishName=bis.'+QUOTENAME(@SecondColumnNameInStaging)+'\r\n" +
                "\r\n\t\t\t\t\t\t';\r\n" +
                "EXECUTE sp_executesql @InsertString;\r\n" +
                "\r\n" +
                "END\r\n" +
                "GO\n" +
                
                "DROP PROCEDURE IF EXISTS OverWriteDatabaseData;\r\n" +
                "GO\r\n" +
                "CREATE PROCEDURE OverWriteDatabaseData\r\n" +
                "@actorId nvarchar(50)\r\n" +
                "AS\r\n" +
                "BEGIN\r\n" +
                "\r\n" +
                "\t--overwriting parents\r\n" +
                "\tEXECUTE uspDeleteAllDataFromDatabase;\r\n" +
                "\tEXECUTE uspTransferDataFromSpecificColumnInStagingTableToEnglishNameColumnInTarget\r\n" +
                "\tN'Bins',\r\n" +
                "\tN'Bin',\r\n" +
                "\t@actorId;\r\n" +
                "\tEXECUTE uspTransferDataFromSpecificColumnInStagingTableToEnglishNameColumnInTarget\r\n" +
                "\tN'Brands',\r\n" +
                "\tN'Brand',\r\n" +
                "\t@actorId;\r\n" +
                "\tEXECUTE uspTransferDataFromSpecificColumnInStagingTableToEnglishNameColumnInTarget\r\n" +
                "\tN'Categories',\r\n" +
                "\tN'Category',\r\n" +
                "\t@actorId;\r\n" +
                "\tEXECUTE uspTransferDataFromSpecificColumnInStagingTableToEnglishNameColumnInTarget\r\n" +
                "\tN'Countries',\r\n" +
                "\tN'Country',\r\n" +
                "\t@actorId;\r\n" +
                "\tEXECUTE uspTransferDataFromSpecificColumnInStagingTableToEnglishNameColumnInTarget\r\n" +
                "\tN'Manufacturers',\r\n" +
                "\tN'Manufacturer',\r\n" +
                "\t@actorId;\r\n" +
                "\tEXECUTE uspTransferDataFromSpecificColumnInStagingTableToEnglishNameColumnInTarget\r\n" +
                "\tN'Units',\r\n" +
                "\tN'Unit',\r\n" +
                "\t@actorId;\r\n" +
                "\tEXECUTE uspTransferDataFromSpecificColumnInStagingTableToEnglishNameColumnInTarget\r\n" +
                "\tN'Suppliers',\r\n" +
                "\tN'Supplier',\r\n" +
                "\t@actorId;\r\n" +
                "\tINSERT INTO Products (Id,SKU,Barcode,ArabicName,\r\n" +
                "\t[Description],[Status],MinStock,VAT,ReorderQTY,Price,CreatorId\r\n" +
                "\t,UpdaterId,DeleterId,UpdatedAt,CreatedAt,DeletedAt,EnglishName)\r\n" +
                "\r\n" +
                "\tSELECT         ROW_NUMBER() OVER (ORDER BY [English Name])\r\n" +
                "\t\t\t\t   + (SELECT ISNULL(MAX(Id), 0) FROM Products ),\r\n" +
                "\t\t\t\t\tSKU, \r\n" +
                "\t\t\t\t\tBarcode,\r\n" +
                "\t\t\t\t\t[Arabic Name],\r\n" +
                "\t\t\t\t\t[Description],\r\n" +
                "\t\t\t\t\t[Status],\r\n" +
                "\t\t\t\t\t[Min Stock],\r\n" +
                "\t\t\t\t\tVAT,\r\n" +
                "\t\t\t\t\t[Reorder Qty],\r\n" +
                "\t\t\t\t\tPrice,\r\n" +
                "\t\t\t\t\t@actorId,\r\n" +
                "\t\t\t\t\t@actorId,\r\n" +
                "\t\t\t\t\tNULL,\r\n" +
                "\t\t\t\t\tSYSDATETIME(),\r\n" +
                "\t\t\t\t\tSYSDATETIME(),\r\n" +
                "\t\t\t\t\t'0001-01-01 00:00:00',\r\n" +
                "\t\t\t\t\t[English Name] -- why not to apply ROW_NUMBER()  here ?\r\n" +
                "\r\n\tFROM (\r\n\t\t\t\t\tSELECT  SKU, \r\n" +
                "\t\t\t\t\t\tBarcode,\r\n" +
                "\t\t\t\t\t\t[Arabic Name],\r\n" +
                "\t\t\t\t\t\t[Description],\r\n" +
                "\t\t\t\t\t\t[Status],\r\n" +
                "\t\t\t\t\t\t[Min Stock],\r\n" +
                "\t\t\t\t\t\tVAT,\r\n" +
                "\t\t\t\t\t\t[Reorder Qty],\r\n" +
                "\t\t\t\t\t\tPrice,\r\n" +
                "\t\t\t\t\t\t[English Name],\r\n" +
                "\t\t\t\t\t\tROW_NUMBER() OVER(PARTITION BY [English Name] ORDER BY [English Name]) rn -- Why not to apply this statement at higher level\r\n" +
                "\t\t\t\t\t FROM BulkInsertedProducts\r\n" +
                "\t\t\t\t  ) a\r\n" +
                "\tWHERE rn = 1;\r\n" +
                "\r\n" +
                "\t--Overwriting junctions\r\n\r\n" +
                "\tEXECUTE TransferDataFromStagingTableToJunctionTable\r\n" +
                "\t\tN'ProductsCategories',\r\n" +
                "\t\tN'ProductId',\r\n" +
                "\t\tN'CategoryId',\r\n" +
                "\t\tN'Products',\r\n" +
                "\t\tN'Categories',\r\n" +
                "\t\tN'English Name',\r\n" +
                "\t\tN'Category';\r\n" +
                "\tEXECUTE TransferDataFromStagingTableToJunctionTable\r\n" +
                "\t\tN'BrandsSuppliers',\r\n" +
                "\t\tN'BrandId',\r\n" +
                "\t\tN'SupplierId',\r\n" +
                "\t\tN'Brands',\r\n" +
                "\t\tN'Suppliers',\r\n" +
                "\t\tN'Brand',\r\n" +
                "\t\tN'Supplier';\r\n" +
                "\tEXECUTE TransferDataFromStagingTableToJunctionTable\r\n" +
                "\t\tN'ProductsBins',\r\n" +
                "\t\tN'ProductId',\r\n" +
                "\t\tN'BinId',\r\n" +
                "\t\tN'Products',\r\n" +
                "\t\tN'Bins',\r\n" +
                "\t\tN'English Name',\r\n" +
                "\t\tN'Bin';\r\n" +
                "\tEXECUTE TransferDataFromStagingTableToJunctionTable\r\n" +
                "\t\tN'ProductsBrands',\r\n" +
                "\t\tN'ProductId',\r\n" +
                "\t\tN'BrandId',\r\n" +
                "\t\tN'Products',\r\n" +
                "\t\tN'Brands',\r\n" +
                "\t\tN'English Name',\r\n" +
                "\t\tN'Brand';\r\n" +
                "\tEXECUTE TransferDataFromStagingTableToJunctionTable\r\n" +
                "\t\tN'ProductsCountries',\r\n" +
                "\t\tN'ProductId',\r\n" +
                "\t\tN'CountryId',\r\n" +
                "\t\tN'Products',\r\n" +
                "\t\tN'Countries',\r\n" +
                "\t\tN'English Name',\r\n" +
                "\t\tN'Country';\r\n" +
                "\tEXECUTE TransferDataFromStagingTableToJunctionTable\r\n" +
                "\t\tN'ProductsManufacturers',\r\n" +
                "\t\tN'ProductId',\r\n" +
                "\t\tN'ManufacturerId',\r\n" +
                "\t\tN'Products',\r\n" +
                "\t\tN'Manufacturers',\r\n" +
                "\t\tN'English Name',\r\n" +
                "\t\tN'Manufacturer';\r\n" +
                "\tEXECUTE TransferDataFromStagingTableToJunctionTable\r\n" +
                "\t\tN'ProductsSuppliers',\r\n" +
                "\t\tN'ProductId',\r\n" +
                "\t\tN'SupplierId',\r\n" +
                "\t\tN'Products',\r\n" +
                "\t\tN'Suppliers',\r\n" +
                "\t\tN'English Name',\r\n" +
                "\t\tN'Supplier';\r\n" +
                "\tEXECUTE TransferDataFromStagingTableToJunctionTable\r\n" +
                "\t\tN'ProductsUnits',\r\n\t\tN'ProductId',\r\n" +
                "\t\tN'UnitId',\r\n\t\tN'Products',\r\n" +
                "\t\tN'Units',\r\n\t\tN'English Name',\r\n" +
                "\t\tN'Unit';\r\n" +
                "\r\n" +
                "\r\n" +
                "    --Overwriting non-junction child(s)\r\n" +
                "\r\n" +
                "\tINSERT INTO SubCategories (Id, ArabicName, CategoryId, CreatorId, UpdaterId, DeleterId, UpdatedAt, CreatedAt, DeletedAt, EnglishName)\r\nSELECT ROW_NUMBER() OVER (ORDER BY Subcategory) + (SELECT ISNULL(MAX(Id), 0)\r\n" +
                "                                                   FROM   SubCategories),\r\n" +
                "       NULL,\r\n" +
                "       p1.Id,\r\n" +
                "       @actorId,\r\n" +
                "       @actorId,\r\n" +
                "       NULL,\r\n" +
                "       SYSDATETIME(),\r\n" +
                "       SYSDATETIME(),\r\n" +
                "       '0001-01-01 00:00:00',\r\n" +
                "       Subcategory\r\n" +
                "FROM   (SELECT Category,\r\n" +
                "               Subcategory,\r\n" +
                "               ROW_NUMBER() OVER (PARTITION BY Subcategory ORDER BY Subcategory) AS rn\r\n" +
                "        FROM   BulkInsertedProducts AS bis) AS a\r\n" +
                "       INNER JOIN\r\n" +
                "       Categories AS p1\r\n" +
                "       ON p1.EnglishName = a.Category\r\n" +
                "WHERE  a.rn = 1;\r\n" +
                "\r\n" +
                "END;\r\n" +
                "GO";
            migrationBuilder.Sql(createStatement);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var dropStatement = "" +

                "DROP PROCEDURE [dbo].[uspDeleteAllDataFromDatabase];" +
                "DROP PROCEDURE [dbo].[uspTransferDataFromSpecificColumnInStagingTableToEnglishNameColumnInTarget];" +
                "DROP PROCEDURE [dbo].[TransferDataFromStagingTableToJunctionTable];" +
                "DROP PROCEDURE [dbo].[OverWriteDatabaseData];" +
                "";
            migrationBuilder.Sql(dropStatement);
        }
    }
}
