-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_AUDITING_MASTER_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [auditing_master_PK]) AS RowNum,
		[auditing_master_PK], [username], [db_name], [table_name], [date], [operation], [server_name]
		FROM [dbo].[AUDITING_MASTER]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[AUDITING_MASTER]
END
