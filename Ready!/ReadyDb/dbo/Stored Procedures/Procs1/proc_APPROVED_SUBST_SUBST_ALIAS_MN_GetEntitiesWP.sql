-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_APPROVED_SUBST_SUBST_ALIAS_MN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [approved_substance_subst_alias_PK]) AS RowNum,
		[approved_substance_subst_alias_PK], [approved_substance_FK], [substance_alias_FK]
		FROM [dbo].[APPROVED_SUBST_SUBST_ALIAS_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[APPROVED_SUBST_SUBST_ALIAS_MN]
END
