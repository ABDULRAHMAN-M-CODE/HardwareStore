USE [HardwareDB]
GO

DROP PROCEDURE uspTransferDataFromSpecificColumnInStagingTableToEnglishNameColumnInTarget;
GO
CREATE PROCEDURE uspTransferDataFromSpecificColumnInStagingTableToEnglishNameColumnInTarget
    @ParentTableName nvarchar(50),
    @ColumnNameInStagingTable nvarchar(50),
    @actorId nvarchar(50)
AS   
BEGIN

    DECLARE @InsertString AS NVARCHAR (500);
    Set @InsertString='
    INSERT INTO '+QUOTENAME(@ParentTableName)+' (Id,CreatorId,UpdaterId,EnglishName,UpdatedAt,CreatedAt,DeletedAt,DeleterId)
    SELECT 
        ROW_NUMBER() OVER (ORDER BY '+QUOTENAME(@ColumnNameInStagingTable)+')
            + (SELECT ISNULL(MAX(Id), 0) FROM '+QUOTENAME(@ParentTableName)+' ),
        @actorId,
        @actorId,
        '+QUOTENAME(@ColumnNameInStagingTable)+',
        SYSDATETIME(),
        SYSDATETIME(),
        ''0001-01-01 00:00:00'',
        NULL
    FROM (SELECT DISTINCT '+QUOTENAME(@ColumnNameInStagingTable)+' FROM BulkInsertedProducts) AS E;
    
    ' ;

EXECUTE sp_executesql
    @InsertString, N'@actorId nvarchar(50)',
    @actorId;

END

GO

