-- Save
CREATE PROCEDURE  proc_XEVPRM_LOG_Save
	@xevprm_log_PK int = NULL,
	@xevprm_message_FK int = NULL,
	@log_time datetime = NULL,
	@description nvarchar(MAX) = NULL,
	@username nvarchar(100) = NULL, 
	@xevprm_status_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[XEVPRM_LOG]
	SET
	[xevprm_message_FK] = @xevprm_message_FK,
	[log_time] = @log_time,
	[description] = @description,
	[username] = @username, 
	[xevprm_status_FK] = @xevprm_status_FK
	WHERE [xevprm_log_PK] = @xevprm_log_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[XEVPRM_LOG]
		([xevprm_message_FK], [log_time], [description], [username], [xevprm_status_FK])
		VALUES
		(@xevprm_message_FK, @log_time, @description, @username, @xevprm_status_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
