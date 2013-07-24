-- Delete
CREATE PROCEDURE  [dbo].[proc_RECIEVED_MESSAGE_Delete]
	@recieved_message_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[RECIEVED_MESSAGE] WHERE [recieved_message_PK] = @recieved_message_PK
END
