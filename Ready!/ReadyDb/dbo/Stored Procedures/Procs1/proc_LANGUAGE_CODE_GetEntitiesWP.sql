-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_LANGUAGE_CODE_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [languagecode_PK]) AS RowNum,
		[languagecode_PK], [name], [code]
		FROM [dbo].[LANGUAGE_CODE]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[LANGUAGE_CODE]
END
