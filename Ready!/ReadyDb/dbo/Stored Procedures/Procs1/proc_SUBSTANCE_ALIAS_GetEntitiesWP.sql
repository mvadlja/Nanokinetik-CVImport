-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_SUBSTANCE_ALIAS_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [substance_alias_PK]) AS RowNum,
		[substance_alias_PK], [sourcecode], [resolutionmode], [aliasname], [substance_aliastranslations_FK]
		FROM [dbo].[SUBSTANCE_ALIAS]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[SUBSTANCE_ALIAS]
END
