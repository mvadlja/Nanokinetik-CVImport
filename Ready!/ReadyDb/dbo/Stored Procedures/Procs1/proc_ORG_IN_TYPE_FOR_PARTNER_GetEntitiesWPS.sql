-- GetEntitiesWPS
CREATE PROCEDURE  [dbo].[proc_ORG_IN_TYPE_FOR_PARTNER_GetEntitiesWPS]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'org_in_type_for_partner_ID'
AS

DECLARE @Query nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		[org_in_type_for_partner_ID], [organization_FK], [org_type_for_partner_FK],[product_FK]
		FROM [dbo].[ORG_IN_TYPE_FOR_PARTNER]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SELECT COUNT(*) FROM [dbo].[ORG_IN_TYPE_FOR_PARTNER]
END
