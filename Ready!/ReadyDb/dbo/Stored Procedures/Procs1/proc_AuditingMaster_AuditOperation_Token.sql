-- =============================================
-- Author:		<Author,,Name>
-- alter date: <alter Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE  [dbo].[proc_AuditingMaster_AuditOperation_Token]
	@Token uniqueidentifier = NULL,
	@DBName [nvarchar](200) = NULL,
	@TableName [nvarchar](200) = NULL,
	@Date [datetime] = NULL,
	@Operation [char](1) = NULL,
	@ServerName [nvarchar](100) = NULL
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
	([Username], [DBName], [TableName], [Date], [Operation], [ServerName])
	VALUES
	(ISNULL(@Username, ''), @DBName, @TableName, @Date, @Operation, @ServerName)
	
	SET @ScopeIdentity = SCOPE_IDENTITY();
	SELECT @ScopeIdentity AS ScopeIdentity
END
