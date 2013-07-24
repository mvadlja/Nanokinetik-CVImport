-- GetEntities
create PROCEDURE proc_RECIEVED_MESSAGE_GetNotProcessedEntitiesPks
	@ReceivedMessageType int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [recieved_message_PK]
	FROM [dbo].[RECIEVED_MESSAGE]
	where (processed = 0 or processed is null)
	and msg_type = @ReceivedMessageType and @ReceivedMessageType IS NOT NULL
	order by [recieved_message_PK]
END