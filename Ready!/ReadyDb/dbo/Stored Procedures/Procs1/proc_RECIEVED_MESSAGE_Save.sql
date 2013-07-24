-- Save
CREATE PROCEDURE  [dbo].[proc_RECIEVED_MESSAGE_Save]
	@recieved_message_PK int = NULL,
	@msg_data varbinary(MAX) = NULL,
	@received_time datetime = NULL,
	@processed_time datetime = NULL,
	@processed bit = NULL,
	@is_successfully_processed bit = NULL,
	@msg_type int = NULL,
	@as_header nvarchar(MAX) = NULL,
	@processing_error nvarchar(MAX) = NULL,
	@xevmpd_FK int = NULL,
	@status int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[RECIEVED_MESSAGE]
	SET
	[msg_data] = @msg_data,
	[received_time] = @received_time,
	[processed_time] = @processed_time,
	[processed] = @processed,
	[is_successfully_processed] = @is_successfully_processed,
	[msg_type] = @msg_type,
	[as_header] = @as_header,
	[processing_error] = @processing_error,
	[xevmpd_FK] = @xevmpd_FK,
	[status] = @status
	WHERE [recieved_message_PK] = @recieved_message_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[RECIEVED_MESSAGE]
		([msg_data], [received_time], [processed_time], [processed], [is_successfully_processed], [msg_type], [as_header], [processing_error], [xevmpd_FK], [status])
		VALUES
		(@msg_data, @received_time, @processed_time, @processed, @is_successfully_processed, @msg_type, @as_header, @processing_error, @xevmpd_FK, @status)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
