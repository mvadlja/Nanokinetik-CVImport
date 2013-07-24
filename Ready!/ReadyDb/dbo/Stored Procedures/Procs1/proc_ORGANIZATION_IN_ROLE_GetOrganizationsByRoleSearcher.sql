-- OrganizationSearcher
CREATE PROCEDURE  [dbo].[proc_ORGANIZATION_IN_ROLE_GetOrganizationsByRoleSearcher]
	@role_name nvarchar(30) = NULL,
	@name_org nvarchar(100) = NULL,
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = ''
AS

DECLARE @Query nvarchar(MAX);
DECLARE @QueryCount nvarchar(MAX);
DECLARE @TempWhereQuery nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		DistinctORGANIZATION.* FROM
		(
			SELECT DISTINCT
			[dbo].[ORGANIZATION].[organization_PK] AS ID, [dbo].[ORGANIZATION].[name_org] AS Name
			FROM [dbo].[ORGANIZATION_IN_ROLE]
			LEFT JOIN [dbo].[ORGANIZATION] ON [dbo].[ORGANIZATION].organization_PK = [dbo].[ORGANIZATION_IN_ROLE].[organization_FK]
			LEFT JOIN [dbo].[ORGANIZATION_ROLE] ON [dbo].[ORGANIZATION_ROLE].role_org_PK = [dbo].[ORGANIZATION_IN_ROLE].[role_org_FK]
			'
			SET @TempWhereQuery = '';

			-- Check nullability for every parameter
			-- @role_name
			IF @role_name IS NOT NULL
			BEGIN
				IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
				ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
				SET @TempWhereQuery = @TempWhereQuery + '[dbo].[ORGANIZATION_ROLE].[role_name] LIKE ''' + CONVERT (nvarchar(MAX), @role_name) + ''''
			END

			-- Check nullability for every parameter
			-- @name_org
			IF @name_org IS NOT NULL
			BEGIN
				IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
				ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
				SET @TempWhereQuery = @TempWhereQuery + '[dbo].[ORGANIZATION].[name_org] LIKE ''%' + REPLACE(@name_org,'[','[[]') + '%'''
			END


			IF LEN(@TempWhereQuery) > 0 BEGIN SET @Query = @Query + @TempWhereQuery; END
			SET @Query = @Query + '
		) DistinctORGANIZATION
	)
	

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	print @query

	EXECUTE sp_executesql @Query;
 
	-- Total count
	SET @QueryCount = 'SELECT COUNT(DISTINCT [dbo].[ORGANIZATION_IN_ROLE].[organization_in_role_ID])
					FROM [dbo].[ORGANIZATION_IN_ROLE]
					LEFT JOIN [dbo].[ORGANIZATION] ON [dbo].[ORGANIZATION].organization_PK = [dbo].[ORGANIZATION_IN_ROLE].[organization_FK]
					LEFT JOIN [dbo].[ORGANIZATION_ROLE] ON [dbo].[ORGANIZATION_ROLE].[role_org_PK] = [dbo].[ORGANIZATION_IN_ROLE].[role_org_FK]
					' + @TempWhereQuery

	EXECUTE sp_executesql @QueryCount;
	
END
