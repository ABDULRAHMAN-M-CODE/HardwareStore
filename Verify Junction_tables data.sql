USE [HardwareDB];


GO
DECLARE @junctionTableName AS VARCHAR (100), @lookupTableName AS VARCHAR (100), @leftColumnNameInExcel AS VARCHAR (100), @rightColumnNameInExcel AS VARCHAR (100), @leftColumnNameInJunctionTable AS VARCHAR (100), @rightColumnNameInJunctionTable AS VARCHAR (100), @parent1Name AS VARCHAR (100), @parent2Name AS VARCHAR (100), @checkInvalidRows AS BIT;

SET @checkInvalidRows = 'true';

DECLARE @i AS INT;

SET @i = 1;

WHILE @i < 9
    BEGIN
        SELECT @junctionTableName = junctionTableName,
               @lookupTableName = lookupTableName,
               @leftColumnNameInExcel = leftColumnNameInExcel,
               @rightColumnNameInExcel = rightColumnNameInExcel,
               @leftColumnNameInJunctionTable = leftColumnNameInJunctionTable,
               @rightColumnNameInJunctionTable = rightColumnNameInJunctionTable,
               @parent1Name = parent1Name,
               @parent2Name = parent2Name
        FROM   tables_and_columns_names
        WHERE  Id = @i;
        EXECUTE inspect_invalid_or_missing_rows_in_junction_table @junctionTableName, @lookupTableName, @leftColumnNameInExcel, @rightColumnNameInExcel, @leftColumnNameInJunctionTable, @rightColumnNameInJunctionTable, @parent1Name, @parent2Name, @checkInvalidRows;
        SET @i = @i + 1;
    END



