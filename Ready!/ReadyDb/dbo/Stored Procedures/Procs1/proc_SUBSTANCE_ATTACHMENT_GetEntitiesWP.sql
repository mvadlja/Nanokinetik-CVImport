-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_SUBSTANCE_ATTACHMENT_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [substance_attachment_PK]) AS RowNum,
		[substance_attachment_PK], [attachmentreference], [resolutionmode], [validitydeclaration]
		FROM [dbo].[SUBSTANCE_ATTACHMENT]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[SUBSTANCE_ATTACHMENT]
END
