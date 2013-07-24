-- GetEntitiesWP
create PROCEDURE [dbo].[proc_MA_SERVICE_LOG_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [ma_service_log_PK]) AS RowNum,
		[ma_service_log_PK], [log_time], [description], [ready_id_FK], [event_type_FK]
		FROM [dbo].[MA_SERVICE_LOG]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[MA_SERVICE_LOG]
END
