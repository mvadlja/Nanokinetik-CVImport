-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_PROJECT_MN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [activity_project_PK]) AS RowNum,
		[activity_project_PK], [activity_FK], [project_FK]
		FROM [dbo].[ACTIVITY_PROJECT_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[ACTIVITY_PROJECT_MN]
END
