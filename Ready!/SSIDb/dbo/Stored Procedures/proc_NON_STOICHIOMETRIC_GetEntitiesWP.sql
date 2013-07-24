
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_NON_STOICHIOMETRIC_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [non_stoichiometric_PK]) AS RowNum,
		[non_stoichiometric_PK], [number_moieties]
		FROM [dbo].[NON_STOICHIOMETRIC]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[NON_STOICHIOMETRIC]
END
