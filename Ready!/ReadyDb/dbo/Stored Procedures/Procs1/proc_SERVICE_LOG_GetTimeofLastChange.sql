-- GetEntity
CREATE PROCEDURE  [dbo].[proc_SERVICE_LOG_GetTimeofLastChange]
	--@service_log_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP 1
	[service_log_PK], [log_time], [description], [user_FK]
	FROM [dbo].[SERVICE_LOG]
	WHERE description LIKE 'Service has been%'
	--WHERE [service_log_PK] = @service_log_PK
	order by  [service_log_PK] desc
END
