-- Save
CREATE PROCEDURE  [dbo].[proc_SERVICE_LOG_Save]
	@service_log_PK int = NULL,
	@log_time datetime = NULL,
	@description nvarchar(MAX) = NULL,
	@user_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[SERVICE_LOG]
	SET
	[log_time] = @log_time,
	[description] = @description,
	[user_FK] = @user_FK
	WHERE [service_log_PK] = @service_log_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[SERVICE_LOG]
		([log_time], [description], [user_FK])
		VALUES
		(@log_time, @description, @user_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
