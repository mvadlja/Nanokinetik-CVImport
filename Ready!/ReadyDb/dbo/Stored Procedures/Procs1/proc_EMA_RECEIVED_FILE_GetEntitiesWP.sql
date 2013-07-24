-- GetEntitiesWP
create PROCEDURE [dbo].[proc_EMA_RECEIVED_FILE_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [ema_received_file_PK]) AS RowNum,
		[ema_received_file_PK], [file_type], [file_data], [xevprm_path], [data_path], [status], [received_time], [processed_time], [as2_from], [as2_to], [as2_header], [mdn_orig_msg_number]
		FROM [dbo].[EMA_RECEIVED_FILE]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[EMA_RECEIVED_FILE]
END
