-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_TASK_NAME_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [task_name_PK]) AS RowNum,
		[task_name_PK], [task_name]
		FROM [dbo].[TASK_NAME]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[TASK_NAME]
END
