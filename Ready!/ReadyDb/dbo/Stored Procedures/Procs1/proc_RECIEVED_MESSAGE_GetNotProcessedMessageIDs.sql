-- GetEntities
CREATE PROCEDURE [dbo].[proc_RECIEVED_MESSAGE_GetNotProcessedMessageIDs]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [recieved_message_PK]
	FROM [dbo].[RECIEVED_MESSAGE]
	where processed = 0 or processed is null
	order by [recieved_message_PK]
END
