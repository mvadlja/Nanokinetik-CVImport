
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_NAME_TYPE_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [substance_name_type_PK]) AS RowNum,
		[substance_name_type_PK], [name]
		FROM [dbo].[SUBSTANCE_NAME_TYPE]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[SUBSTANCE_NAME_TYPE]
END
