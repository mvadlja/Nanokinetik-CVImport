-- Delete
CREATE PROCEDURE  [dbo].[proc_SENT_MESSAGE_Delete]
	@sent_message_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[SENT_MESSAGE] WHERE [sent_message_PK] = @sent_message_PK
END
