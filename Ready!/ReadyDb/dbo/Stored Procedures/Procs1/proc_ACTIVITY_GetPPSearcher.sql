-- ProductPISearcher
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_GetPPSearcher]
	@name nvarchar(100) = NULL,
	@applicant nvarchar(100) = null,
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
			activity_PK AS ID,
			[dbo].[ReturnActivitySearcherName](activity_PK) AS Name,
			 ''Applicant'' = CASE
				WHEN submission_date = '''' OR submission_date IS NULL THEN applicant.name_org
				ELSE applicant.name_org + '' ('' + CONVERT(nvarchar(max),submission_date,104) + '')''
			 END
			FROM [dbo].[ACTIVITY]
			LEFT JOIN [dbo].[ACTIVITY_ORGANIZATION_APP_MN] applicant_MN ON applicant_MN.activity_FK=activity_PK
			LEFT JOIN [dbo].[ORGANIZATION] applicant ON applicant.organization_PK=applicant_MN.organization_FK
			'
			SET @TempWhereQuery = '';


			-- Check nullability for every parameter
			-- @name_org
			IF @name IS NOT NULL
			BEGIN
				IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
				ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
				SET @TempWhereQuery = @TempWhereQuery + '[dbo].[ReturnActivitySearcherName](activity_PK) LIKE ''%' + REPLACE(@name,'[','[[]') + '%'''
			END
			
			--@applicant
			IF @applicant IS NOT NULL
			BEGIN
				IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
				ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
				SET @TempWhereQuery = @TempWhereQuery + '(CASE
				WHEN submission_date = '''' OR submission_date IS NULL THEN applicant.name_org
				ELSE applicant.name_org  + CONVERT(nvarchar(max),submission_date,104) 
				END) LIKE ''%' + REPLACE(@applicant,'[','[[]') + '%'''
														 
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
	SET @QueryCount = 'SELECT COUNT(DISTINCT [activity_PK]) 
						FROM [dbo].[ACTIVITY]
						LEFT JOIN [dbo].[ACTIVITY_ORGANIZATION_APP_MN] applicant_MN ON applicant_MN.activity_FK=activity_PK
						LEFT JOIN [dbo].[ORGANIZATION] applicant ON applicant.organization_PK=applicant_MN.organization_FK
					  ' + @TempWhereQuery

	EXECUTE sp_executesql @QueryCount;
	
END
