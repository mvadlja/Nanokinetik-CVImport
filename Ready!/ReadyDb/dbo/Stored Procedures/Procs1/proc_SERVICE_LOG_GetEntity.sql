-- GetEntity
CREATE PROCEDURE  [dbo].[proc_SERVICE_LOG_GetEntity]
	@service_log_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[service_log_PK], [log_time], [description], [user_FK]
	FROM [dbo].[SERVICE_LOG]
	WHERE [service_log_PK] = @service_log_PK
END
