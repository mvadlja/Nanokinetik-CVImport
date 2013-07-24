-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_TYPE_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [type_PK]) AS RowNum,
		[dbo].[TYPE].*
		FROM [dbo].[TYPE]
		WHERE [dbo].[TYPE].[group] NOT IN ('DTD', 'M', 'P', 'PP', 'SU', 'T', 'TU', '0')
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[TYPE]
END
