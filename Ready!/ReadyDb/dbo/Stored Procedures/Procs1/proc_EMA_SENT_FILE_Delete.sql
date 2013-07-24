-- Delete
create PROCEDURE [dbo].[proc_EMA_SENT_FILE_Delete]
	@ema_sent_file_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[EMA_SENT_FILE] WHERE [ema_sent_file_PK] = @ema_sent_file_PK
END
