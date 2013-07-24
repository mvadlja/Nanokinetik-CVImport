-- =============================================
-- Author:		<Author,,Name>
-- alter date: <alter Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE  [dbo].[proc_AUDITING_MASTER_AuditOperation_Token]
	@Token uniqueidentifier = NULL,
	@db_name [nvarchar](200) = NULL,
	@table_name [nvarchar](200) = NULL,
	@date [datetime] = NULL,
	@operation [char](1) = NULL,
	@server_name [nvarchar](100) = NULL
AS
BEGIN
	DECLARE @Username nvarchar(100);
	SET @Username = 
	(
		SELECT Username FROM Users
		LEFT JOIN CurrentLogonSessions ON Users.IDUser = CurrentLogonSessions.UserID
		WHERE CurrentLogonSessions.Token = @Token
	);

	DECLARE @ScopeIdentity int

	INSERT INTO [dbo].[AuditingMaster]
	([username], [DBName], [TableName], [date], [operation], [ServerName])
	VALUES
	(ISNULL(@Username, ''), @db_name, @table_name, @date, @operation, @server_name)
	
	SET @ScopeIdentity = SCOPE_IDENTITY();
	SELECT @ScopeIdentity AS ScopeIdentity
END
