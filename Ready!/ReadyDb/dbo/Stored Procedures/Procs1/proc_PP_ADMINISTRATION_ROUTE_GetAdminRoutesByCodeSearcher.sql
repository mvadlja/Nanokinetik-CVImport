-- OrganizationSearcher
CREATE PROCEDURE  [dbo].[proc_PP_ADMINISTRATION_ROUTE_GetAdminRoutesByCodeSearcher]
	@code nvarchar(60) = NULL,
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
		DistinctPPADMINROUTES.* FROM
		(
			SELECT DISTINCT
			[dbo].[PP_ADMINISTRATION_ROUTE].[adminroute_PK] AS ID, [dbo].[PP_ADMINISTRATION_ROUTE].[adminroutecode] AS Name
			FROM [dbo].[PP_ADMINISTRATION_ROUTE]
			'
			SET @TempWhereQuery = '';

			-- Check nullability for every parameter
			-- @code
			IF @code IS NOT NULL
			BEGIN
				IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
				ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
				SET @TempWhereQuery = @TempWhereQuery + '[dbo].[PP_ADMINISTRATION_ROUTE].[adminroutecode] LIKE ''%' + REPLACE(@code,'[','[[]') + '%'''
			END


			IF LEN(@TempWhereQuery) > 0 BEGIN SET @Query = @Query + @TempWhereQuery; END
			SET @Query = @Query + '
		) DistinctPPADMINROUTES
	)
	

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;
 
	-- Total count
	SET @QueryCount = 'SELECT COUNT(DISTINCT [dbo].[PP_ADMINISTRATION_ROUTE].[adminroute_PK])
					FROM [dbo].[PP_ADMINISTRATION_ROUTE]
					' + @TempWhereQuery

	EXECUTE sp_executesql @QueryCount;
	
END
