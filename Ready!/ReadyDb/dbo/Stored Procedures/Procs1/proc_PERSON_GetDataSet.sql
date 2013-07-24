CREATE PROCEDURE  [dbo].[proc_PERSON_GetDataSet]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'person_PK'
AS

DECLARE @Query nvarchar(MAX);
DECLARE @QueryCount nvarchar(MAX);
DECLARE @TempWhereQuery nvarchar(MAX);
DECLARE @Temp nvarchar(200);
DECLARE @dash nvarchar(5);
SET @dash='''-''';

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		DistinctPerson.* FROM
		(
			SELECT [PERSON].*,
			c.abbreviation +'+@dash +'+c.name as country
			FROM [dbo].[PERSON]
			LEFT JOIN [dbo].[COUNTRY] c ON c.country_PK = [dbo].[PERSON].[country_FK]
			'
			SET @TempWhereQuery = '';
			
			-- Check nullability for every parameter
			-- @product_FK
			
			IF LEN(@TempWhereQuery) > 0 BEGIN SET @Query = @Query + @TempWhereQuery; END
			SET @Query = @Query + '
		) DistinctPerson
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SET @QueryCount = '
	SELECT COUNT(DISTINCT [dbo].[PERSON].[person_PK]) FROM [dbo].[PERSON]
	' + @TempWhereQuery

	EXECUTE sp_executesql @QueryCount;
END
