-- ProductsSearcher
CREATE PROCEDURE  [dbo].[proc_AUTHORISED_PRODUCT_ProductsSearcher]
	@name nvarchar(2000) = NULL,
	@description nvarchar(2500) = NULL,
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = ''
AS

DECLARE @Query nvarchar(MAX);
DECLARE @QueryCount nvarchar(MAX);
DECLARE @TempWhereQuery nvarchar(MAX); 

BEGIN
	SET NOCOUNT ON;
	
	if (@OrderByQuery like 'Pack%ASC') 
	 set @OrderByQuery = '[Packaging description] ASC';
	if (@OrderByQuery like 'Pack%DESC') 
	 set @OrderByQuery = '[Packaging description] DESC';
	
	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		DistinctPRODUCT.* FROM
		(
			SELECT DISTINCT
			[dbo].[AUTHORISED_PRODUCT].[ap_PK] as ID, 
			CASE 
				WHEN [dbo].[AUTHORISED_PRODUCT].[authorisationcountrycode_FK] IS NOT NULL AND [authorisationnumber] IS NOT NULL THEN ([dbo].[AUTHORISED_PRODUCT].[product_name] + '' ('' + c.abbreviation + ISNULL(NULLIF('', '' + [authorisationnumber], '', ''), '''') + '')'')
				WHEN [dbo].[AUTHORISED_PRODUCT].[authorisationcountrycode_FK] IS NULL AND [authorisationnumber] IS NOT NULL THEN ([dbo].[AUTHORISED_PRODUCT].[product_name] +  ISNULL(NULLIF('' (-, '' + [authorisationnumber], '' (-, ''), '''') + '')'')
				WHEN [dbo].[AUTHORISED_PRODUCT].[authorisationcountrycode_FK] IS NOT NULL AND [authorisationnumber] IS NULL THEN ([dbo].[AUTHORISED_PRODUCT].[product_name] + '' ('' + c.abbreviation + '')'')
				WHEN [dbo].[AUTHORISED_PRODUCT].[authorisationcountrycode_FK] IS NULL AND [authorisationnumber] IS NULL THEN [dbo].[AUTHORISED_PRODUCT].[product_name]
			END as Name,
			[dbo].[AUTHORISED_PRODUCT].packagedesc AS ''Packaging description''
			FROM [dbo].[AUTHORISED_PRODUCT]
			LEFT JOIN [dbo].[COUNTRY] AS c ON [dbo].[AUTHORISED_PRODUCT].[authorisationcountrycode_FK] = c.[country_PK]
			'
			SET @TempWhereQuery = '';

			-- Check nullability for every parameter
			-- @name
			IF @name IS NOT NULL
			BEGIN
				IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
				ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
				SET @TempWhereQuery = @TempWhereQuery + '
				CASE 
					WHEN [dbo].[AUTHORISED_PRODUCT].[authorisationcountrycode_FK] IS NOT NULL AND [authorisationnumber] IS NOT NULL THEN ([dbo].[AUTHORISED_PRODUCT].[product_name] + '' ('' + c.abbreviation + '', '' + [authorisationnumber] + '' )'')
					WHEN [dbo].[AUTHORISED_PRODUCT].[authorisationcountrycode_FK] IS NULL AND [authorisationnumber] IS NOT NULL THEN ([dbo].[AUTHORISED_PRODUCT].[product_name] + '' (-, '' + [authorisationnumber] + '' )'')
					WHEN [dbo].[AUTHORISED_PRODUCT].[authorisationcountrycode_FK] IS NOT NULL AND [authorisationnumber] IS NULL THEN ([dbo].[AUTHORISED_PRODUCT].[product_name] + '' ('' + c.abbreviation + '', -)'')
					WHEN [dbo].[AUTHORISED_PRODUCT].[authorisationcountrycode_FK] IS NULL AND [authorisationnumber] IS NULL THEN [dbo].[AUTHORISED_PRODUCT].[product_name]
				END LIKE ''%' + REPLACE(@name,'[','[[]') + '%'''
			END

			-- @description
			IF @description IS NOT NULL
			BEGIN
				IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
				ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
				SET @TempWhereQuery = @TempWhereQuery + '[dbo].[AUTHORISED_PRODUCT].packagedesc LIKE ''%' + REPLACE(@description,'[','[[]') + '%'''
			END

			IF LEN(@TempWhereQuery) > 0 BEGIN SET @Query = @Query + @TempWhereQuery; END
			SET @Query = @Query + '
		) DistinctPRODUCT
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

print @Query;
	EXECUTE sp_executesql @Query;

	-- Total count
	SET @QueryCount = '
	SELECT COUNT(DISTINCT [dbo].[AUTHORISED_PRODUCT].[ap_PK]) FROM 
	[dbo].[AUTHORISED_PRODUCT]
	JOIN [dbo].[COUNTRY] AS c ON [dbo].[AUTHORISED_PRODUCT].[authorisationcountrycode_FK]=c.[country_PK]
	' + @TempWhereQuery

	EXECUTE sp_executesql @QueryCount;
END
