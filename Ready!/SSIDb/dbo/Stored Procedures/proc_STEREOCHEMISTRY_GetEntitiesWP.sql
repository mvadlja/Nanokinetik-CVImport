
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_STEREOCHEMISTRY_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [stereochemistry_PK]) AS RowNum,
		[stereochemistry_PK], [name]
		FROM [dbo].[STEREOCHEMISTRY]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[STEREOCHEMISTRY]
END
