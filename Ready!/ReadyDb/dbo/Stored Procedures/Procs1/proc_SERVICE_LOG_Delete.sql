-- Delete
CREATE PROCEDURE  [dbo].[proc_SERVICE_LOG_Delete]
	@service_log_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[SERVICE_LOG] WHERE [service_log_PK] = @service_log_PK
END
