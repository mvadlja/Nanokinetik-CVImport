
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_SUBTYPE_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [subtype_PK]) AS RowNum,
		[subtype_PK], [substance_class_subtype]
		FROM [dbo].[SUBTYPE]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[SUBTYPE]
END
