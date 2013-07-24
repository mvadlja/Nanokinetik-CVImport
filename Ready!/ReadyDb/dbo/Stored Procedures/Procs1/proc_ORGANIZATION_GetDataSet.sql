CREATE PROCEDURE  [dbo].[proc_ORGANIZATION_GetDataSet]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'organization_PK'
AS

DECLARE @Query nvarchar(MAX);
DECLARE @QueryCount nvarchar(MAX);
DECLARE @TempWhereQuery nvarchar(MAX);
DECLARE @Temp nvarchar(200);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		DistinctORGANIZATION.* FROM
		(
			SELECT [dbo].[ORGANIZATION].*,
			country.abbreviation AS country_code
			FROM [dbo].[ORGANIZATION] 
			LEFT JOIN [dbo].[COUNTRY] country ON country.country_PK=[dbo].[ORGANIZATION].countrycode_FK
			'
			SET @TempWhereQuery = '';
			
			-- Check nullability for every parameter
			-- @product_FK
			
			IF LEN(@TempWhereQuery) > 0 BEGIN SET @Query = @Query + @TempWhereQuery; END
			SET @Query = @Query + '
		) DistinctORGANIZATION
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SET @QueryCount = '
	SELECT COUNT(DISTINCT [dbo].[ORGANIZATION].[organization_PK]) FROM [dbo].[ORGANIZATION]
	' + @TempWhereQuery

	EXECUTE sp_executesql @QueryCount;
END
