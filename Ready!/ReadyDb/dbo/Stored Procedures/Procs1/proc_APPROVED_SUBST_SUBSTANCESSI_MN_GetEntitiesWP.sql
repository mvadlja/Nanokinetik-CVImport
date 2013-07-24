-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_APPROVED_SUBST_SUBSTANCESSI_MN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [approved_subst_substancessi_PK]) AS RowNum,
		[approved_subst_substancessi_PK], [approved_substance_FK], [substancessi_FK]
		FROM [dbo].[APPROVED_SUBST_SUBSTANCESSI_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[APPROVED_SUBST_SUBSTANCESSI_MN]
END
