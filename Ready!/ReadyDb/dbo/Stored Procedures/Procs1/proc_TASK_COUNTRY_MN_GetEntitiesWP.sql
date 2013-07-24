-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_TASK_COUNTRY_MN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [task_country_PK]) AS RowNum,
		[task_country_PK], [task_FK], [country_FK]
		FROM [dbo].[TASK_COUNTRY_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[TASK_COUNTRY_MN]
END
