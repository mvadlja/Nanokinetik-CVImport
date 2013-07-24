-- Save
CREATE PROCEDURE  [dbo].[proc_AS2_HANDLER_LOG_Save]
	@as2_handler_log_PK int = NULL,
	@log_time datetime = NULL,
	@event_type nvarchar(50) = NULL,
	@description nvarchar(MAX) = NULL,
	@received_time datetime = NULL,
	@message_number nvarchar(100) = NULL,
	@as2_to nvarchar(50) = NULL,
	@as2_from nvarchar(50) = NULL,
	@message_ID nvarchar(100) = NULL,
	@filename nvarchar(50) = NULL,
	@received_message_FK int = NULL,
	@connection nvarchar(50) = NULL,
	@date nvarchar(50) = NULL,
	@content_length nvarchar(50) = NULL,
	@content_type nvarchar(100) = NULL,
	@from nvarchar(100) = NULL,
	@host nvarchar(50) = NULL,
	@user_agent nvarchar(50) = NULL,
	@mime_version nvarchar(50) = NULL,
	@content_transfer_encoding nvarchar(50) = NULL,
	@content_disposition nvarchar(50) = NULL,
	@disposition_notification_to nvarchar(50) = NULL,
	@disposition_notification_options nvarchar(100) = NULL,
	@receipt_delivery_option nvarchar(100) = NULL,
	@ediint_features nvarchar(50) = NULL,
	@as2_version nvarchar(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[AS2_HANDLER_LOG]
	SET
	[log_time] = @log_time,
	[event_type] = @event_type,
	[description] = @description,
	[received_time] = @received_time,
	[message_number] = @message_number,
	[as2_to] = @as2_to,
	[as2_from] = @as2_from,
	[message_ID] = @message_ID,
	[filename] = @filename,
	[received_message_FK] = @received_message_FK,
	[connection] = @connection,
	[date] = @date,
	[content_length] = @content_length,
	[content_type] = @content_type,
	[from] = @from,
	[host] = @host,
	[user_agent] = @user_agent,
	[mime_version] = @mime_version,
	[content_transfer_encoding] = @content_transfer_encoding,
	[content_disposition] = @content_disposition,
	[disposition_notification_to] = @disposition_notification_to,
	[disposition_notification_options] = @disposition_notification_options,
	[receipt_delivery_option] = @receipt_delivery_option,
	[ediint_features] = @ediint_features,
	[as2_version] = @as2_version
	WHERE [as2_handler_log_PK] = @as2_handler_log_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[AS2_HANDLER_LOG]
		([log_time], [event_type], [description], [received_time], [message_number], [as2_to], [as2_from], [message_ID], [filename], [received_message_FK], [connection], [date], [content_length], [content_type], [from], [host], [user_agent], [mime_version], [content_transfer_encoding], [content_disposition], [disposition_notification_to], [disposition_notification_options], [receipt_delivery_option], [ediint_features], [as2_version])
		VALUES
		({fn NOW()}, @event_type, @description, @received_time, @message_number, @as2_to, @as2_from, @message_ID, @filename, @received_message_FK, @connection, @date, @content_length, @content_type, @from, @host, @user_agent, @mime_version, @content_transfer_encoding, @content_disposition, @disposition_notification_to, @disposition_notification_options, @receipt_delivery_option, @ediint_features, @as2_version)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
