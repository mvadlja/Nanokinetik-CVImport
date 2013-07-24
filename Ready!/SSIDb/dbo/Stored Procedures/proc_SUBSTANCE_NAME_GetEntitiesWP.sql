
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_NAME_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [substance_name_PK]) AS RowNum,
		[substance_name_PK], [subst_name], [subst_name_type_FK], [language_FK]
		FROM [dbo].[SUBSTANCE_NAME]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[SUBSTANCE_NAME]
END
