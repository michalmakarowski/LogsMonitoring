DECLARE @tableName NVARCHAR(128)
DECLARE @sql NVARCHAR(MAX)

-- Create a cursor to iterate over the list of tables
DECLARE tableCursor CURSOR FOR
SELECT name
FROM sys.tables WHERE name NOT IN ('SchemaVersions', 'AuditLogs');

-- Open the cursor
OPEN tableCursor

-- Fetch the first table name
FETCH NEXT FROM tableCursor INTO @tableName

-- Loop through each table and create a trigger
WHILE @@FETCH_STATUS = 0
BEGIN
    SET @sql = N'CREATE TRIGGER AuditTrigger_' + @tableName + '
                 ON ' + @tableName + '
    AFTER INSERT, UPDATE, DELETE
AS
BEGIN
	EXEC AuditInsertProcedure
END'

    -- Execute the dynamically generated CREATE TRIGGER statement
    EXEC sp_executesql @sql

    -- Fetch the next table name
    FETCH NEXT FROM tableCursor INTO @tableName
END

-- Close and deallocate the cursor
CLOSE tableCursor
DEALLOCATE tableCursor