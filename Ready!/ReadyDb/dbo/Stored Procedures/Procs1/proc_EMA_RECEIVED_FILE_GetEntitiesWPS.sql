-- GetEntitiesWPS
create PROCEDURE [dbo].[proc_EMA_RECEIVED_FILE_GetEntitiesWPS]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'ema_received_file_PK'
AS

DECLARE @Query nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		[ema_received_file_PK], [file_type], [file_data], [xevprm_path], [data_path], [status], [received_time], [processed_time], [as2_from], [as2_to], [as2_header], [mdn_orig_msg_number]
		FROM [dbo].[EMA_RECEIVED_FILE]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SELECT COUNT(*) FROM [dbo].[EMA_RECEIVED_FILE]
END
