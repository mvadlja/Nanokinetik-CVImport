-- GetEntitiesWP
create PROCEDURE [dbo].[proc_EMA_SENT_FILE_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [ema_sent_file_PK]) AS RowNum,
		[ema_sent_file_PK], [file_name], [file_type], [file_data], [status], [sent_time], [as_to], [as2_from], [as2_header]
		FROM [dbo].[EMA_SENT_FILE]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[EMA_SENT_FILE]
END
