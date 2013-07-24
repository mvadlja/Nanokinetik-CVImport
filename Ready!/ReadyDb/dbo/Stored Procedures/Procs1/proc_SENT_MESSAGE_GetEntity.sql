-- GetEntity
CREATE PROCEDURE  [dbo].[proc_SENT_MESSAGE_GetEntity]
	@sent_message_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[sent_message_PK], [msg_data], [sent_time], [msg_type], [xevmpd_FK]
	FROM [dbo].[SENT_MESSAGE]
	WHERE [sent_message_PK] = @sent_message_PK
END
