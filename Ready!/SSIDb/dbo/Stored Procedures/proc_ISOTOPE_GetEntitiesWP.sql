
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_ISOTOPE_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [isotope_PK]) AS RowNum,
		[isotope_PK], [nuclide_id], [nuclide_name], [substitution_type]
		FROM [dbo].[ISOTOPE]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[ISOTOPE]
END
