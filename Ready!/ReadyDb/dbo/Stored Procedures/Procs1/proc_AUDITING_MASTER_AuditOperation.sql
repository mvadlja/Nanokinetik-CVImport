-- =============================================
-- Author:		<Author,,Name>
-- alter date: <alter Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE  [dbo].[proc_AUDITING_MASTER_AuditOperation]
	@username [nvarchar](100) = NULL,
	@db_name [nvarchar](200) = NULL,
	@table_name [nvarchar](200) = NULL,
	@date [datetime] = NULL,
	@operation [char](1) = NULL,
	@server_name [nvarchar](100) = NULL
AS
BEGIN
	DECLARE @ScopeIdentity int

	INSERT INTO [dbo].[AUDITING_MASTER]
	([username], [db_name], [table_name], [date], [operation], [server_name])
	VALUES
	(@username, @db_name, @table_name, @date, @operation, @server_name)
	
	SET @ScopeIdentity = SCOPE_IDENTITY();
	SELECT @ScopeIdentity AS ScopeIdentity
END
