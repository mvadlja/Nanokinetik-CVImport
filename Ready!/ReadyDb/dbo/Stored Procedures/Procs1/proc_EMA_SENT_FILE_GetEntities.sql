-- GetEntities
create PROCEDURE [dbo].[proc_EMA_SENT_FILE_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[ema_sent_file_PK], [file_name], [file_type], [file_data], [status], [sent_time], [as_to], [as2_from], [as2_header]
	FROM [dbo].[EMA_SENT_FILE]
END
