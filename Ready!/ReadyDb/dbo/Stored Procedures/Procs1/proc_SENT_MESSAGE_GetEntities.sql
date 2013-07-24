-- GetEntities
CREATE PROCEDURE  [dbo].[proc_SENT_MESSAGE_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[sent_message_PK], [msg_data], [sent_time], [msg_type], [xevmpd_FK]
	FROM [dbo].[SENT_MESSAGE]
END
