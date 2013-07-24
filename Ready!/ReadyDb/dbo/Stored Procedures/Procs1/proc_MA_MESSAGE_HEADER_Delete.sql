-- Delete
create PROCEDURE  [dbo].[proc_MA_MESSAGE_HEADER_Delete]
	@ma_message_header_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[MA_MESSAGE_HEADER] WHERE [ma_message_header_PK] = @ma_message_header_PK
END
