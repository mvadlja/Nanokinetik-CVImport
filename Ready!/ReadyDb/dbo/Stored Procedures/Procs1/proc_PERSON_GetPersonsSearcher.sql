-- ProductsSearcher
CREATE PROCEDURE  [dbo].[proc_PERSON_GetPersonsSearcher]
	@name nvarchar(50) = NULL,
	@email nvarchar(25) = NULL,
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'name'
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
		DistinctPERSONS.* FROM
		(
			SELECT DISTINCT
			[dbo].[PERSON].person_PK AS ID, [dbo].[PERSON].FullName AS Name, [dbo].[PERSON].email as Email
			FROM [dbo].[PERSON] 
			'
			SET @TempWhereQuery = '';

			-- Check nullability for every parameter
			-- @role_name
			IF @name IS NOT NULL
			BEGIN
				IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
				ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
				SET @TempWhereQuery = @TempWhereQuery + '([dbo].[PERSON].name + '' '' + [dbo].[PERSON].familyname) LIKE ''%' + REPLACE(@name,'[','[[]') + '%'''
			END


			-- Check nullability for every parameter
			-- @name
			IF @email IS NOT NULL
			BEGIN
				IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
				ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
				SET @TempWhereQuery = @TempWhereQuery + '[dbo].[PERSON].email LIKE ''%' + REPLACE(@email,'[','[[]') + '%'''
			END

			

			IF LEN(@TempWhereQuery) > 0 BEGIN SET @Query = @Query + @TempWhereQuery; END
			SET @Query = @Query + '
		) DistinctPERSONS
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	print @Query;
	EXECUTE sp_executesql @Query;

	-- Total count
	SET @QueryCount = 'SELECT COUNT(DISTINCT [dbo].[PERSON].[person_PK])
					FROM [dbo].[PERSON] 
					' + @TempWhereQuery

	EXECUTE sp_executesql @QueryCount;
END
