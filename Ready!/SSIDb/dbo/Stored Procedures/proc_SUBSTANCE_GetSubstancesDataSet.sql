

-- GetAuthorisedProductsDataSet
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_GetSubstancesDataSet]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'substance_s_PK'
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
		Distinct_SUBSTANCE.* FROM
		(
			SELECT DISTINCT
			[dbo].[SUBSTANCE].*
			FROM [dbo].[SUBSTANCE]
			'
			SET @TempWhereQuery = '';

			-- Check nullability for every parameter
			IF LEN(@TempWhereQuery) > 0 BEGIN SET @Query = @Query + @TempWhereQuery; END
			SET @Query = @Query + '
		) Distinct_SUBSTANCE
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SET @QueryCount = '
	SELECT COUNT(DISTINCT [dbo].[SUBSTANCE].[substance_s_PK]) FROM [dbo].[SUBSTANCE]
	' + @TempWhereQuery

	EXECUTE sp_executesql @QueryCount;
END

