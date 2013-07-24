-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_ORGANIZATION_ROLE_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [role_org_PK]) AS RowNum,
		[role_org_PK], [role_number], [role_name]
		FROM [dbo].[ORGANIZATION_ROLE]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[ORGANIZATION_ROLE]
END
