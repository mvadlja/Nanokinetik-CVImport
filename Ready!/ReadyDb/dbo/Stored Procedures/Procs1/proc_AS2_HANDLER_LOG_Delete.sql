-- Delete
CREATE PROCEDURE  [dbo].[proc_AS2_HANDLER_LOG_Delete]
	@as2_handler_log_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[AS2_HANDLER_LOG] WHERE [as2_handler_log_PK] = @as2_handler_log_PK
END
