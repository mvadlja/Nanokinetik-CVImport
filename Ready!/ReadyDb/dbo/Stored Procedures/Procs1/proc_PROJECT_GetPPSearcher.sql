-- ProductPISearcher
CREATE PROCEDURE  [dbo].[proc_PROJECT_GetPPSearcher]
	@name nvarchar(100) = NULL,
	@internalStatus nvarchar(100) = NULL,
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
			project_PK AS ID, 
				CASE 
					WHEN [dbo].[ReturnProjectCountries](project_PK) IS NULL THEN [dbo].[PROJECT].[name] 
					ELSE [dbo].[PROJECT].[name] + '' ('' + [dbo].[ReturnProjectCountries](project_PK) + '')'' 
				END AS Name,
			is_type.name AS InternalStatus
			FROM [dbo].[PROJECT]
			LEFT JOIN [dbo].[TYPE] is_type ON internal_status_type_FK = is_type.type_PK
			'
			SET @TempWhereQuery = '';


			-- Check nullability for every parameter
			-- @name_org
			IF @name IS NOT NULL
			BEGIN
				IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
				ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
				SET @TempWhereQuery = @TempWhereQuery + 'CASE 
					WHEN [dbo].[ReturnProjectCountries](project_PK) IS NULL THEN [dbo].[PROJECT].[name] 
					ELSE [dbo].[PROJECT].[name] + ''('' + [dbo].[ReturnProjectCountries](project_PK) + '')'' 
				END LIKE ''%' + REPLACE(@name,'[','[[]') + '%'''
			END
			
			--@internalStatus
			if @internalStatus IS NOT NULL
			BEGIN
				IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
				ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
				SET @TempWhereQuery = @TempWhereQuery + 'is_type.name LIKE ''%' + REPLACE(@internalStatus,'[','[[]') + '%'''
			END


			IF LEN(@TempWhereQuery) > 0 BEGIN SET @Query = @Query + @TempWhereQuery; END
			SET @Query = @Query + '
		) DistinctPP
	)
	

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;
	print @Query;
	-- Total count
	SET @QueryCount = 'SELECT COUNT(DISTINCT [project_PK]) FROM [dbo].[PROJECT]
						LEFT JOIN [dbo].[TYPE] is_type ON internal_status_type_FK = is_type.type_PK
					  ' + @TempWhereQuery

	EXECUTE sp_executesql @QueryCount;
	
END
