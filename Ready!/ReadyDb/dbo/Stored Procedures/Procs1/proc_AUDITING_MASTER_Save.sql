-- Save
CREATE PROCEDURE  [dbo].[proc_AUDITING_MASTER_Save]
	@auditing_master_PK int = NULL,
	@username nvarchar(100) = NULL,
	@db_name nvarchar(200) = NULL,
	@table_name nvarchar(200) = NULL,
	@date datetime = NULL,
	@operation char(1) = NULL,
	@server_name nvarchar(100) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[AUDITING_MASTER]
	SET
	[username] = @username,
	[db_name] = @db_name,
	[table_name] = @table_name,
	[date] = @date,
	[operation] = @operation,
	[server_name] = @server_name
	WHERE [auditing_master_PK] = @auditing_master_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[AUDITING_MASTER]
		([username], [db_name], [table_name], [date], [operation], [server_name])
		VALUES
		(@username, @db_name, @table_name, @date, @operation, @server_name)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
