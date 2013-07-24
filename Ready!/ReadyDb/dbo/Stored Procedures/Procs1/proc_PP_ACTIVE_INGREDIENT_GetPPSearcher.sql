-- [proc_TASK_GetPPSearcher]
CREATE PROCEDURE  [dbo].[proc_PP_ACTIVE_INGREDIENT_GetPPSearcher]
	@name nvarchar(100) = NULL,
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
		DistinctPP.* FROM
		(
			SELECT DISTINCT
			activeingredient_PK AS ID, 
			''Name''=CASE
			WHEN concise='''' or concise is null then sub.substance_name
			else concise 
			END
			FROM [dbo].PP_ACTIVE_INGREDIENT
			join SUBSTANCES as sub on sub.substance_PK = substancecode_FK
			'
			SET @TempWhereQuery = '';


			-- Check nullability for every parameter
			-- @name_org
			IF @name IS NOT NULL
			BEGIN
				IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
				ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
				SET @TempWhereQuery = @TempWhereQuery + 'concise LIKE ''%' + REPLACE(@name,'[','[[]') + '%'''
			END


			IF LEN(@TempWhereQuery) > 0 BEGIN SET @Query = @Query + @TempWhereQuery; END
			SET @Query = @Query + '
		) DistinctPP
	)
	

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	print @query

	EXECUTE sp_executesql @Query;
 
	-- Total count
	SET @QueryCount = 'SELECT COUNT(DISTINCT [activeingredient_PK]) FROM [dbo].PP_ACTIVE_INGREDIENT
					  ' + @TempWhereQuery

	EXECUTE sp_executesql @QueryCount;
	
END
