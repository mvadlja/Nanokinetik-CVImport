-- DocumentsSearcher
create PROCEDURE  [dbo].[proc_DOCUMENT_DocumentsSearcher]
	@name nvarchar(2000) = NULL,
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'document_PK'
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
		DistinctDOCUMENT.* FROM
		(
			SELECT DISTINCT
			[dbo].[DOCUMENT].[document_PK] as ID, 
			[dbo].[DOCUMENT].[name] as Name
			FROM [dbo].[DOCUMENT]
			'
			SET @TempWhereQuery = '';

			-- Check nullability for every parameter
			-- @name
			IF @name IS NOT NULL
			BEGIN
				IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
				ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
				SET @TempWhereQuery = @TempWhereQuery + '[dbo].[DOCUMENT].[name] LIKE ''%' + REPLACE(@name,'[','[[]') + '%'''
			END

			IF LEN(@TempWhereQuery) > 0 BEGIN SET @Query = @Query + @TempWhereQuery; END
			SET @Query = @Query + '
		) DistinctDOCUMENT
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SET @QueryCount = '
	SELECT COUNT(DISTINCT [dbo].[DOCUMENT].[document_PK]) FROM [dbo].[DOCUMENT]
	' + @TempWhereQuery

	EXECUTE sp_executesql @QueryCount;
END