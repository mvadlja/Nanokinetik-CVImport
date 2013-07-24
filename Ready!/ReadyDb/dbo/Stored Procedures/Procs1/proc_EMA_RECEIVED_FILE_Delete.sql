-- Delete
create PROCEDURE [dbo].[proc_EMA_RECEIVED_FILE_Delete]
	@ema_received_file_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[EMA_RECEIVED_FILE] WHERE [ema_received_file_PK] = @ema_received_file_PK
END
