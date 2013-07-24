
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_SING_STRUCTURE_MN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [sing_structure_mn_PK]) AS RowNum,
		[sing_structure_mn_PK], [sing_FK], [structure_FK]
		FROM [dbo].[SING_STRUCTURE_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[SING_STRUCTURE_MN]
END
