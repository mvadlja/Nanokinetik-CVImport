-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_PP_DOCUMENT_MN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [pp_document_PK]) AS RowNum,
		[pp_document_PK], [pp_FK], [doc_FK]
		FROM [dbo].[PP_DOCUMENT_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[PP_DOCUMENT_MN]
END
