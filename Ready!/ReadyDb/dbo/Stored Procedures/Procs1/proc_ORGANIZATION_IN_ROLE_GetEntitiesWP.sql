-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_ORGANIZATION_IN_ROLE_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [organization_in_role_ID]) AS RowNum,
		[organization_in_role_ID], [organization_FK], [role_org_FK]
		FROM [dbo].[ORGANIZATION_IN_ROLE]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[ORGANIZATION_IN_ROLE]
END
