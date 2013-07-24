-- GetEntities
CREATE PROCEDURE  [dbo].[proc_SERVICE_LOG_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[service_log_PK], [log_time], [description], [user_FK]
	FROM [dbo].[SERVICE_LOG]
END
