CREATE PROCEDURE PrintProcedure
AS
BEGIN
	DECLARE @sqlCmd nvarchar(128)
	DECLARE @tmp TABLE (
        EventType NVARCHAR(30), 
        [Parameters] INT, 
        EventInfo NVARCHAR(MAX))

	INSERT @tmp EXEC('DBCC INPUTBUFFER(@@SPID)')

	SET @sqlCmd = (SELECT EventInfo FROM @tmp)

	DECLARE @operation NVARCHAR(128) = 'UNKNOWN'
	IF (CharIndex('INSERT', @sqlCmd) = 1)
		SET @operation = 'INSERT'

	PRINT @sqlCmd

	insert into [dbo].[AuditLogs] (OperationType, date, Message) VALUES (@operation, GETDATE(),@sqlCmd)
END