
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_REFERENCE_INFORMATION_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [reference_info_PK]) AS RowNum,
		[reference_info_PK], [comment]
		FROM [dbo].[REFERENCE_INFORMATION]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[REFERENCE_INFORMATION]
END
