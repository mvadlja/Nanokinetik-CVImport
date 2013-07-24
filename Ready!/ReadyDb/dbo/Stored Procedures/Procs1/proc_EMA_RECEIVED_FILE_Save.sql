-- Save
create PROCEDURE [dbo].[proc_EMA_RECEIVED_FILE_Save]
	@ema_received_file_PK int = NULL,
	@file_type nvarchar(50) = NULL,
	@file_data varbinary(MAX) = NULL,
	@xevprm_path nvarchar(1000) = NULL,
	@data_path nvarchar(1000) = NULL,
	@status int = NULL,
	@received_time datetime = NULL,
	@processed_time datetime = NULL,
	@as2_from nvarchar(100) = NULL,
	@as2_to nvarchar(100) = NULL,
	@as2_header nvarchar(MAX) = NULL,
	@mdn_orig_msg_number nvarchar(100) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[EMA_RECEIVED_FILE]
	SET
	[file_type] = @file_type,
	[file_data] = @file_data,
	[xevprm_path] = @xevprm_path,
	[data_path] = @data_path,
	[status] = @status,
	[received_time] = @received_time,
	[processed_time] = @processed_time,
	[as2_from] = @as2_from,
	[as2_to] = @as2_to,
	[as2_header] = @as2_header,
	[mdn_orig_msg_number] = @mdn_orig_msg_number
	WHERE [ema_received_file_PK] = @ema_received_file_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[EMA_RECEIVED_FILE]
		([file_type], [file_data], [xevprm_path], [data_path], [status], [received_time], [processed_time], [as2_from], [as2_to], [as2_header], [mdn_orig_msg_number])
		VALUES
		(@file_type, @file_data, @xevprm_path, @data_path, @status, @received_time, @processed_time, @as2_from, @as2_to, @as2_header, @mdn_orig_msg_number)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
