-- GetEntities
CREATE PROCEDURE  [dbo].[proc_AS2_HANDLER_LOG_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[as2_handler_log_PK], [log_time], [event_type], [description], [received_time], [message_number], [as2_to], [as2_from], [message_ID], [filename], [received_message_FK], [connection], [date], [content_length], [content_type], [from], [host], [user_agent], [mime_version], [content_transfer_encoding], [content_disposition], [disposition_notification_to], [disposition_notification_options], [receipt_delivery_option], [ediint_features], [as2_version]
	FROM [dbo].[AS2_HANDLER_LOG]
END
