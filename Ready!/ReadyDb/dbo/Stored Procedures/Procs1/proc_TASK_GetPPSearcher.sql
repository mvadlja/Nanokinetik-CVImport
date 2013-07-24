-- [proc_TASK_GetPPSearcher]
CREATE PROCEDURE  [dbo].[proc_TASK_GetPPSearcher]
	@name nvarchar(100) = NULL,
	@activity nvarchar(max) = NULL,
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
			task_PK AS ID, 
			CASE WHEN [dbo].[ReturnTaskCountries](task_PK) IS NULL THEN t.task_name
				ELSE t.task_name + '' ('' + [dbo].[ReturnTaskCountries](task_PK) + '')''
			END AS Name,
			[dbo].[ReturnActivitySearcherName](activity_FK) AS Activity
			FROM [dbo].[TASK]
			INNER JOIN [dbo].[TASK_NAME] t ON t.task_name_PK = [dbo].[TASK].[task_name_FK]
			'
			SET @TempWhereQuery = '';


			-- Check nullability for every parameter
			-- @name_org
			IF @name IS NOT NULL
			BEGIN
				IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
				ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
				SET @TempWhereQuery = @TempWhereQuery + 'CASE WHEN [dbo].[ReturnTaskCountries](task_PK) IS NULL THEN t.task_name
															ELSE t.task_name + '' ('' + [dbo].[ReturnTaskCountries](task_PK) + '')''
														 END LIKE ''%' + REPLACE(@name,'[','[[]') + '%'''
			END
			
			-- @activity
			IF @activity IS NOT NULL
			BEGIN
				IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
				ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
				SET @TempWhereQuery = @TempWhereQuery + '[dbo].[ReturnActivitySearcherName](activity_FK) LIKE ''%' + REPLACE(@activity,'[','[[]') + '%'''
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
	SET @QueryCount = 'SELECT COUNT(DISTINCT [task_PK]) FROM [dbo].[TASK]
					   LEFT JOIN [dbo].[TASK_NAME] t ON t.task_name_PK = [dbo].[TASK].[task_name_FK]
					  ' + @TempWhereQuery

	EXECUTE sp_executesql @QueryCount;
	
END
