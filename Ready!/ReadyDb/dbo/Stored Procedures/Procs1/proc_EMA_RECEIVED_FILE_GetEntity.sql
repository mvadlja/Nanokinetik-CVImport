-- GetEntity
create PROCEDURE [dbo].[proc_EMA_RECEIVED_FILE_GetEntity]
	@ema_received_file_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[ema_received_file_PK], [file_type], [file_data], [xevprm_path], [data_path], [status], [received_time], [processed_time], [as2_from], [as2_to], [as2_header], [mdn_orig_msg_number]
	FROM [dbo].[EMA_RECEIVED_FILE]
	WHERE [ema_received_file_PK] = @ema_received_file_PK
END
