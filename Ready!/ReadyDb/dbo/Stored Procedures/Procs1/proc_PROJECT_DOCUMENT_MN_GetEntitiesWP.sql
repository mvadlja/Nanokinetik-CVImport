-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_PROJECT_DOCUMENT_MN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [project_document_PK]) AS RowNum,
		[project_document_PK], [project_FK], [document_FK]
		FROM [dbo].[PROJECT_DOCUMENT_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[PROJECT_DOCUMENT_MN]
END
