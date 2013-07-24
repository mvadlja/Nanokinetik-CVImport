-- Delete
create PROCEDURE [dbo].[proc_MA_SERVICE_LOG_Delete]
	@ma_service_log_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[MA_SERVICE_LOG] WHERE [ma_service_log_PK] = @ma_service_log_PK
END
