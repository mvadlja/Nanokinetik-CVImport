-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_SERVICE_LOG_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [service_log_PK]) AS RowNum,
		[service_log_PK], [log_time], [description], [user_FK]
		FROM [dbo].[SERVICE_LOG]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[SERVICE_LOG]
END
