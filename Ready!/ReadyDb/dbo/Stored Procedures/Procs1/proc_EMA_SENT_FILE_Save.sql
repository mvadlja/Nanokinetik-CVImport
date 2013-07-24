-- Save
create PROCEDURE [dbo].[proc_EMA_SENT_FILE_Save]
	@ema_sent_file_PK int = NULL,
	@file_name nvarchar(500) = NULL,
	@file_type nvarchar(50) = NULL,
	@file_data varbinary(MAX) = NULL,
	@status int = NULL,
	@sent_time datetime = NULL,
	@as_to nvarchar(50) = NULL,
	@as2_from nvarchar(50) = NULL,
	@as2_header nvarchar(MAX) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[EMA_SENT_FILE]
	SET
	[file_name] = @file_name,
	[file_type] = @file_type,
	[file_data] = @file_data,
	[status] = @status,
	[sent_time] = @sent_time,
	[as_to] = @as_to,
	[as2_from] = @as2_from,
	[as2_header] = @as2_header
	WHERE [ema_sent_file_PK] = @ema_sent_file_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[EMA_SENT_FILE]
		([file_name], [file_type], [file_data], [status], [sent_time], [as_to], [as2_from], [as2_header])
		VALUES
		(@file_name, @file_type, @file_data, @status, @sent_time, @as_to, @as2_from, @as2_header)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
