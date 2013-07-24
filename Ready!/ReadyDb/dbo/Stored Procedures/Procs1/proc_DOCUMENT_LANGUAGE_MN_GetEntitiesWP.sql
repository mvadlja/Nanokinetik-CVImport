-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_DOCUMENT_LANGUAGE_MN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [document_language_mn_PK]) AS RowNum,
		[document_language_mn_PK], [document_FK], [language_FK]
		FROM [dbo].[DOCUMENT_LANGUAGE_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[DOCUMENT_LANGUAGE_MN]
END
