CREATE TABLE [dbo].[AuditLogs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OperationType] [varchar](50) NOT NULL,
	[Date] [dateTime] NOT NULL,
	[Message] [text] NULL,
	[ExecutingUser] [varchar](50) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]