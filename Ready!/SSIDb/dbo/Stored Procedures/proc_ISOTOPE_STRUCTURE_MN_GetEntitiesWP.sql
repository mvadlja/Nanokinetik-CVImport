
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_ISOTOPE_STRUCTURE_MN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [isotope_structure_mn_PK]) AS RowNum,
		[isotope_structure_mn_PK], [isotope_FK], [structure_FK]
		FROM [dbo].[ISOTOPE_STRUCTURE_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[ISOTOPE_STRUCTURE_MN]
END
