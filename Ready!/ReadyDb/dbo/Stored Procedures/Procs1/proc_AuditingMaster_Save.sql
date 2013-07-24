-- Save
CREATE PROCEDURE  [dbo].[proc_AuditingMaster_Save]
	@IDAuditingMaster int = NULL,
	@Username nvarchar(100) = NULL,
	@DBName nvarchar(200) = NULL,
	@TableName nvarchar(200) = NULL,
	@Date datetime = NULL,
	@Operation char(1) = NULL,
	@ServerName nvarchar(100) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[AuditingMaster]
	SET
	[Username] = @Username,
	[DBName] = @DBName,
	[TableName] = @TableName,
	[Date] = @Date,
	[Operation] = @Operation,
	[ServerName] = @ServerName
	WHERE [IDAuditingMaster] = @IDAuditingMaster

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[AuditingMaster]
		([Username], [DBName], [TableName], [Date], [Operation], [ServerName])
		VALUES
		(@Username, @DBName, @TableName, @Date, @Operation, @ServerName)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
