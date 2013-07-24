-- GetEntity
create PROCEDURE [dbo].[proc_MA_SERVICE_LOG_GetEntity]
	@ma_service_log_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[ma_service_log_PK], [log_time], [description], [ready_id_FK], [event_type_FK]
	FROM [dbo].[MA_SERVICE_LOG]
	WHERE [ma_service_log_PK] = @ma_service_log_PK
END
