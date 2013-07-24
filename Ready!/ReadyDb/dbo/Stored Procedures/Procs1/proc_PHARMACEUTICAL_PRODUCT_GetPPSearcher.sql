-- ProductPISearcher
CREATE PROCEDURE  [dbo].[proc_PHARMACEUTICAL_PRODUCT_GetPPSearcher]
	@name nvarchar(100) = NULL,
	@concise nvarchar(100) = NULL,
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
		DistinctPP.* FROM
		(
			SELECT DISTINCT
			[pharmaceutical_product_PK] AS ID, 
			ISNULL((CASE WHEN [ID] IS NULL OR [ID] = '''' THEN [name]
				ELSE [name] + '' ('' + [ID] + '')''
			END), ''Missing'') AS Name,
			dbo.[ReturnPPIngrStrForm]([dbo].[PHARMACEUTICAL_PRODUCT].pharmaceutical_product_PK) AS Concise
			FROM [dbo].[PHARMACEUTICAL_PRODUCT]
			'
			SET @TempWhereQuery = '';

			-- Check nullability for every parameter
			-- @name_org
			IF @name IS NOT NULL
			BEGIN
				IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
				ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
				SET @TempWhereQuery = @TempWhereQuery + '
				(CASE WHEN [ID] IS NULL OR [ID] = '''' THEN ISNULL([name], ''Missing'')
					ELSE [name] + ''('' + [ID] + '')''
				END) LIKE ''%' + REPLACE(@name,'[','[[]') + '%'''
			END
			
			-- @concise
			IF @concise IS NOT NULL AND @concise != ''
			BEGIN
				IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
				ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
				SET @TempWhereQuery = @TempWhereQuery + 'dbo.[ReturnPPIngrStrForm]([dbo].[PHARMACEUTICAL_PRODUCT].pharmaceutical_product_PK) LIKE ''%' + REPLACE(@concise,'[','[[]') + '%'''
			END


			IF LEN(@TempWhereQuery) > 0 BEGIN SET @Query = @Query + @TempWhereQuery; END
			SET @Query = @Query + '
		) DistinctPP
	)
	

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;
	PRINT @Query;
 
	-- Total count
	SET @QueryCount = 'SELECT COUNT(DISTINCT [pharmaceutical_product_PK]) FROM [dbo].[PHARMACEUTICAL_PRODUCT]
					  ' + @TempWhereQuery

	EXECUTE sp_executesql @QueryCount;
	
END
