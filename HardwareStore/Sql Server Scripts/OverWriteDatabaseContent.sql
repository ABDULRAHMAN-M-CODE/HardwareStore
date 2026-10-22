USE [HardwareDB]
GO

DROP PROCEDURE IF EXISTS OverWriteDatabaseData;
GO
CREATE PROCEDURE OverWriteDatabaseData
@actorId nvarchar(50)
AS
BEGIN

	--overwriting parents
	EXECUTE uspDeleteAllDataFromDatabase;
	EXECUTE uspTransferDataFromSpecificColumnInStagingTableToEnglishNameColumnInTarget
	N'Bins',
	N'Bin',
	@actorId;
	EXECUTE uspTransferDataFromSpecificColumnInStagingTableToEnglishNameColumnInTarget
	N'Brands',
	N'Brand',
	@actorId;
	EXECUTE uspTransferDataFromSpecificColumnInStagingTableToEnglishNameColumnInTarget
	N'Categories',
	N'Category',
	@actorId;
	EXECUTE uspTransferDataFromSpecificColumnInStagingTableToEnglishNameColumnInTarget
	N'Countries',
	N'Country',
	@actorId;
	EXECUTE uspTransferDataFromSpecificColumnInStagingTableToEnglishNameColumnInTarget
	N'Manufacturers',
	N'Manufacturer',
	@actorId;
	EXECUTE uspTransferDataFromSpecificColumnInStagingTableToEnglishNameColumnInTarget
	N'Units',
	N'Unit',
	@actorId;
	EXECUTE uspTransferDataFromSpecificColumnInStagingTableToEnglishNameColumnInTarget
	N'Suppliers',
	N'Supplier',
	@actorId;
	INSERT INTO Products (Id,SKU,Barcode,ArabicName,
	[Description],[Status],MinStock,VAT,ReorderQTY,Price,CreatorId
	,UpdaterId,DeleterId,UpdatedAt,CreatedAt,DeletedAt,EnglishName)

	SELECT         ROW_NUMBER() OVER (ORDER BY [English Name])
				   + (SELECT ISNULL(MAX(Id), 0) FROM Products ),
					SKU, 
					Barcode,
					[Arabic Name],
					[Description],
					[Status],
					[Min Stock],
					VAT,
					[Reorder Qty],
					Price,
					@actorId,
					@actorId,
					NULL,
					SYSDATETIME(),
					SYSDATETIME(),
					'0001-01-01 00:00:00',
					[English Name] -- why not to apply ROW_NUMBER()  here ?

	FROM (
					SELECT  SKU, 
						Barcode,
						[Arabic Name],
						[Description],
						[Status],
						[Min Stock],
						VAT,
						[Reorder Qty],
						Price,
						[English Name],
						ROW_NUMBER() OVER(PARTITION BY [English Name] ORDER BY [English Name]) rn -- Why not to apply this statement at higher level
					 FROM BulkInsertedProducts
				  ) a
	WHERE rn = 1;

	--Overwriting junctions

	EXECUTE TransferDataFromStagingTableToJunctionTable
		N'ProductsCategories',
		N'ProductId',
		N'CategoryId',
		N'Products',
		N'Categories',
		N'English Name',
		N'Category';
	EXECUTE TransferDataFromStagingTableToJunctionTable
		N'BrandsSuppliers',
		N'BrandId',
		N'SupplierId',
		N'Brands',
		N'Suppliers',
		N'Brand',
		N'Supplier';
	EXECUTE TransferDataFromStagingTableToJunctionTable
		N'ProductsBins',
		N'ProductId',
		N'BinId',
		N'Products',
		N'Bins',
		N'English Name',
		N'Bin';
	EXECUTE TransferDataFromStagingTableToJunctionTable
		N'ProductsBrands',
		N'ProductId',
		N'BrandId',
		N'Products',
		N'Brands',
		N'English Name',
		N'Brand';
	EXECUTE TransferDataFromStagingTableToJunctionTable
		N'ProductsCountries',
		N'ProductId',
		N'CountryId',
		N'Products',
		N'Countries',
		N'English Name',
		N'Country';
	EXECUTE TransferDataFromStagingTableToJunctionTable
		N'ProductsManufacturers',
		N'ProductId',
		N'ManufacturerId',
		N'Products',
		N'Manufacturers',
		N'English Name',
		N'Manufacturer';
	EXECUTE TransferDataFromStagingTableToJunctionTable
		N'ProductsSuppliers',
		N'ProductId',
		N'SupplierId',
		N'Products',
		N'Suppliers',
		N'English Name',
		N'Supplier';
	EXECUTE TransferDataFromStagingTableToJunctionTable
		N'ProductsUnits',
		N'ProductId',
		N'UnitId',
		N'Products',
		N'Units',
		N'English Name',
		N'Unit';


    --Overwriting non-junction child(s)

	INSERT INTO SubCategories (Id, ArabicName, CategoryId, CreatorId, UpdaterId, DeleterId, UpdatedAt, CreatedAt, DeletedAt, EnglishName)
SELECT ROW_NUMBER() OVER (ORDER BY Subcategory) + (SELECT ISNULL(MAX(Id), 0)
                                                   FROM   SubCategories),
       NULL,
       p1.Id,
       @actorId,
       @actorId,
       NULL,
       SYSDATETIME(),
       SYSDATETIME(),
       '0001-01-01 00:00:00',
       Subcategory
FROM   (SELECT Category,
               Subcategory,
               ROW_NUMBER() OVER (PARTITION BY Subcategory ORDER BY Subcategory) AS rn
        FROM   BulkInsertedProducts AS bis) AS a
       INNER JOIN
       Categories AS p1
       ON p1.EnglishName = a.Category
WHERE  a.rn = 1;

END;
GO

EXECUTE OverWriteDatabaseData
N'a1f28e89-2ba1-493e-b005-744decdd78b3';
GO

