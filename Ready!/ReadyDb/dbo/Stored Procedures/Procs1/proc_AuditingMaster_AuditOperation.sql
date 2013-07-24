-- =============================================
-- Author:		<Author,,Name>
-- alter date: <alter Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE  [dbo].[proc_AuditingMaster_AuditOperation]
	@Username [nvarchar](100) = NULL,
	@DBName [nvarchar](200) = NULL,
	@TableName [nvarchar](200) = NULL,
	@Date [datetime] = NULL,
	@Operation [char](1) = NULL,
	@ServerName [nvarchar](100) = NULL,
	@SessionToken [nvarchar](50) = NULL
AS
BEGIN
	DECLARE @ScopeIdentity int

	INSERT INTO [dbo].[AuditingMaster]
	([Username], [DBName], [TableName], [Date], [Operation], [ServerName], [SessionToken])
	VALUES
	(@Username, @DBName, @TableName, @Date, @Operation, @ServerName, @SessionToken)
	
	SET @ScopeIdentity = SCOPE_IDENTITY();
	SELECT @ScopeIdentity AS ScopeIdentity
END
