-- GetEntities
CREATE PROCEDURE  proc_XEVPRM_LOG_GetEntities
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[xevprm_log_PK], [xevprm_message_FK], [log_time], [description], [username], [xevprm_status_FK]
	FROM [dbo].[XEVPRM_LOG]
END
