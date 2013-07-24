-- Delete
CREATE PROCEDURE  [dbo].[proc_XEVPRM_LOG_Delete]
	@xevprm_log_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[XEVPRM_LOG] WHERE [xevprm_log_PK] = @xevprm_log_PK
END
