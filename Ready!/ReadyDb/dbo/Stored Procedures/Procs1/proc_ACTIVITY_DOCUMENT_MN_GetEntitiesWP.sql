-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_DOCUMENT_MN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [activity_document_PK]) AS RowNum,
		[activity_document_PK], [activity_FK], [document_FK]
		FROM [dbo].[ACTIVITY_DOCUMENT_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[ACTIVITY_DOCUMENT_MN]
END
