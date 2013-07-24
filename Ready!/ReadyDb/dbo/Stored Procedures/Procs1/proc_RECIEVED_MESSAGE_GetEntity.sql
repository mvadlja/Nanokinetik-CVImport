-- GetEntity
CREATE PROCEDURE  [dbo].[proc_RECIEVED_MESSAGE_GetEntity]
	@recieved_message_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[recieved_message_PK], [msg_data], [received_time], [processed_time], [processed], [is_successfully_processed], [msg_type], [as_header], [processing_error], [xevmpd_FK], [status]
	FROM [dbo].[RECIEVED_MESSAGE]
	WHERE [recieved_message_PK] = @recieved_message_PK
END
