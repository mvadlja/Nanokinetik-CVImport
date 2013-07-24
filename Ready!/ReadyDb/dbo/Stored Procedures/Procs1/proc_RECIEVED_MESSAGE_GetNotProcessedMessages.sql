-- GetEntities
CREATE PROCEDURE [dbo].[proc_RECIEVED_MESSAGE_GetNotProcessedMessages]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM [dbo].[RECIEVED_MESSAGE]
	where processed = 0 or processed is null
	order by [recieved_message_PK]
END
