
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_CHEMICAL_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [chemical_PK]) AS RowNum,
		[chemical_PK], [stoichiometric], [comment], [non_stoichio_FK]
		FROM [dbo].[CHEMICAL]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[CHEMICAL]
END
