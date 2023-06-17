CREATE PROCEDURE [dbo].[AuditInsertProcedure]
AS
BEGIN
	DECLARE @sqlCmd nvarchar(128)
	DECLARE @tmp TABLE (
        EventType NVARCHAR(30), 
        [Parameters] INT, 
        EventInfo NVARCHAR(MAX))

	INSERT @tmp EXEC('DBCC INPUTBUFFER(@@SPID)')

	SET @sqlCmd = (SELECT trim(EventInfo) FROM @tmp)

	DECLARE @operation NVARCHAR(128) = 'UNKNOWN'
	IF (CharIndex('INSERT', @sqlCmd) >= 1)
		SET @operation = 'INSERT'
	else if (CharIndex('UPDATE', @sqlCmd) >= 1)
		SET @operation = 'UPDATE'
	else if (CharIndex('DELETE', @sqlCmd) >= 1)
		SET @operation = 'DELETE'

	insert into [dbo].[AuditLogs] (OperationType, date, Message,ExecutingUser) VALUES (@operation, GETDATE(),@sqlCmd, SUSER_NAME())
END