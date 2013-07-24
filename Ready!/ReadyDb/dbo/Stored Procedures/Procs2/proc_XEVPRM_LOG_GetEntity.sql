-- GetEntity
CREATE PROCEDURE  proc_XEVPRM_LOG_GetEntity
	@xevprm_log_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[xevprm_log_PK], [xevprm_message_FK], [log_time], [description], [username], [xevprm_status_FK]
	FROM [dbo].[XEVPRM_LOG]
	WHERE [xevprm_log_PK] = @xevprm_log_PK
END
