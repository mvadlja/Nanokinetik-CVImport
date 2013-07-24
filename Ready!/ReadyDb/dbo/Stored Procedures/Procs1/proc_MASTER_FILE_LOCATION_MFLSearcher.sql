-- ProductsSearcher
CREATE PROCEDURE  [dbo].[proc_MASTER_FILE_LOCATION_MFLSearcher]
	@name nvarchar(2000) = NULL,
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
		DistinctMFL.* FROM
		(
			SELECT DISTINCT
			[dbo].[MASTER_FILE_LOCATION].[master_file_location_PK] as ID, [dbo].[MASTER_FILE_LOCATION].[mflcompany] AS Name
			FROM [dbo].[MASTER_FILE_LOCATION]
			'
			SET @TempWhereQuery = '';

			-- Check nullability for every parameter
			-- @name
			IF @name IS NOT NULL
			BEGIN
				IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
				ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
				SET @TempWhereQuery = @TempWhereQuery + '[dbo].[MASTER_FILE_LOCATION].[mflcompany] LIKE ''%' + REPLACE(@name,'[','[[]') + '%'''
			END

			---- @description
			--IF @description IS NOT NULL
			--BEGIN
			--	IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			--	ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			--	SET @TempWhereQuery = @TempWhereQuery + '[dbo].[PRODUCT].[description] LIKE ''%' + REPLACE(@description,'[','[[]') + '%'''
			--END

			IF LEN(@TempWhereQuery) > 0 BEGIN SET @Query = @Query + @TempWhereQuery; END
			SET @Query = @Query + '
		) DistinctMFL
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SET @QueryCount = '
	SELECT COUNT(DISTINCT [dbo].[MASTER_FILE_LOCATION].[master_file_location_PK]) FROM [dbo].[MASTER_FILE_LOCATION]
	' + @TempWhereQuery

	EXECUTE sp_executesql @QueryCount;
END
