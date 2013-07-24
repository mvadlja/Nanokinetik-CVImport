-- ProductsSearcher
CREATE PROCEDURE  proc_PRODUCT_ProductsSearcher
	@name nvarchar(2000) = NULL,
	@countries nvarchar(2500) = NULL,
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'ID'
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
		DistinctPRODUCT.* FROM
		(
			SELECT DISTINCT
			[dbo].[PRODUCT].[product_PK] as ID, 
			''Name'' =
				CASE
				WHEN ([dbo].[PRODUCT].product_number = '''' or [dbo].[PRODUCT].product_number is null)
					AND ([dbo].[PRODUCT].[name] <> '''' AND [dbo].[PRODUCT].[name] is not null) THEN [dbo].[PRODUCT].[name]
					
				WHEN ([dbo].[PRODUCT].product_number <> '''' or [dbo].[PRODUCT].product_number is not null)
					AND ([dbo].[PRODUCT].[name] <> '''' or [dbo].[PRODUCT].[name] is not null) 
					THEN [dbo].[PRODUCT].[name]+'' (''+[dbo].[PRODUCT].product_number+'')''
					
				WHEN ([dbo].[PRODUCT].product_number = '''' or [dbo].[PRODUCT].product_number is null)
					AND ([dbo].[PRODUCT].[name] ='''' or [dbo].[PRODUCT].[name] is null) THEN ''Missing''
					
				WHEN ([dbo].[PRODUCT].product_number <> '''' or [dbo].[PRODUCT].product_number is not null)
					AND ([dbo].[PRODUCT].[name] = '''' AND [dbo].[PRODUCT].[name] is  null) 
					THEN ''Missing''+'' (''+[dbo].[PRODUCT].product_number+'')''
				END
			, dbo.[ReturnProductCountries]([dbo].[PRODUCT].product_PK) as Countries
			FROM [dbo].[PRODUCT]
			'
			SET @TempWhereQuery = '';

			-- Check nullability for every parameter
			-- @name
			IF @name IS NOT NULL
			BEGIN
				IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
				ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
				SET @TempWhereQuery = @TempWhereQuery + '((([dbo].[PRODUCT].[product_number] is null OR [dbo].[PRODUCT].[product_number]='''') AND [dbo].[PRODUCT].[name] LIKE ''%' + REPLACE(@name,'[','[[]') + '%'')
														 OR
														 ([dbo].[PRODUCT].[product_number] is not null AND [dbo].[PRODUCT].[product_number]<>'''' AND ([dbo].[PRODUCT].[name]+'' (''+[dbo].[PRODUCT].product_number+'')'') LIKE ''%' + REPLACE(@name,'[','[[]') + '%''))';
			END
			

			-- @description
			IF @countries IS NOT NULL
			BEGIN
				IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
				ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
				SET @TempWhereQuery = @TempWhereQuery + 'Countries LIKE ''%' + REPLACE(@countries,'[','[[]') + '%'''
			END

			IF LEN(@TempWhereQuery) > 0 BEGIN SET @Query = @Query + @TempWhereQuery; END
			SET @Query = @Query + '
		) DistinctPRODUCT
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SET @QueryCount = '
	SELECT COUNT(DISTINCT [dbo].[PRODUCT].[product_PK]) FROM [dbo].[PRODUCT]
	' + @TempWhereQuery

	EXECUTE sp_executesql @QueryCount;
END
