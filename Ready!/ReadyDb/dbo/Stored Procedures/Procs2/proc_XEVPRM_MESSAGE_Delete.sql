-- Delete
CREATE PROCEDURE  [dbo].[proc_XEVPRM_MESSAGE_Delete]
	@xevprm_message_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[XEVPRM_MESSAGE] WHERE [xevprm_message_PK] = @xevprm_message_PK
END
