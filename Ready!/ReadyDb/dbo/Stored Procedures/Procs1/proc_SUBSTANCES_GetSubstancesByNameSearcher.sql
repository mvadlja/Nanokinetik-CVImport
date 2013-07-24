-- OrganizationSearcher
CREATE PROCEDURE  [dbo].[proc_SUBSTANCES_GetSubstancesByNameSearcher]
	@name nvarchar(60) = NULL,
	@evcode nvarchar(30) = NULL,
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'Name'
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
		DistinctSUBSTANCES.* FROM
		(
			SELECT DISTINCT
			[dbo].[SUBSTANCES].[substance_PK] AS ID, 
			[dbo].[SUBSTANCES].[substance_name] AS Name,
			CASE 
				WHEN [dbo].[SUBSTANCES].[ev_code] IS NOT NULL THEN [dbo].[SUBSTANCES].[ev_code]
				ELSE ''Missing''
			END AS EVCODE
			FROM [dbo].[SUBSTANCES]
			'
			SET @TempWhereQuery = '';

			-- Check nullability for every parameter
			-- @code
			IF @name IS NOT NULL
			BEGIN
				IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
				ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
				SET @TempWhereQuery = @TempWhereQuery + '[dbo].[SUBSTANCES].[substance_name] LIKE ''' + REPLACE(@name,'[','[[]') + '%'''
			END
			
			-- @evcode
			IF @evcode IS NOT NULL AND @evcode != ''
			BEGIN
				IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
				ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
				SET @TempWhereQuery = @TempWhereQuery + '[dbo].[SUBSTANCES].[ev_code] LIKE ''%' + REPLACE(@evcode,'[','[[]') + '%'''
			END


			IF LEN(@TempWhereQuery) > 0 BEGIN SET @Query = @Query + @TempWhereQuery; END
			SET @Query = @Query + '
		) DistinctSUBSTANCES
	)
	

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;
 
	-- Total count
	SET @QueryCount = 'SELECT COUNT(DISTINCT [dbo].[SUBSTANCES].[substance_PK]) FROM [dbo].[SUBSTANCES]
					' + @TempWhereQuery

	EXECUTE sp_executesql @QueryCount;
	
END
