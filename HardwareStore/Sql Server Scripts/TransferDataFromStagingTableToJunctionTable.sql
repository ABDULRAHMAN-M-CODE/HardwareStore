DROP PROCEDURE IF EXISTS TransferDataFromStagingTableToJunctionTable;
go
CREATE PROCEDURE TransferDataFromStagingTableToJunctionTable
    @JunctionTableName nvarchar(50),
	@FirstColumnNameInjunctionTable nvarchar(50),
	@SecondColumnNameInjunctionTable nvarchar(50),
	@FirstParentTableName nvarchar(50),
	@SecondParentTableName nvarchar(50),
	@FirstColumnNameInStaging nvarchar(50),
	@SecondColumnNameInStaging nvarchar(50)
AS 
BEGIN
DECLARE @InsertString AS NVARCHAR (500);

SET @InsertString = 
						'
							SET NOCOUNT ON;
							INSERT INTO '+QUOTENAME(@JunctionTableName)+'('+QUOTENAME(@FirstColumnNameInjunctionTable)+', '+QUOTENAME(@SecondColumnNameInjunctionTable)+')
							SELECT DISTINCT p1.Id as '+QUOTENAME(@FirstColumnNameInjunctionTable)+',p2.Id as '+QUOTENAME(@SecondColumnNameInjunctionTable)+'
							FROM BulkInsertedProducts bis
							INNER JOIN '+QUOTENAME(@FirstParentTableName)+' p1 ON p1.EnglishName=bis.'+QUOTENAME(@FirstColumnNameInStaging)+'
							INNER JOIN '+QUOTENAME(@SecondParentTableName)+' p2 on p2.EnglishName=bis.'+QUOTENAME(@SecondColumnNameInStaging)+'

						';
EXECUTE sp_executesql @InsertString;

END
GO
