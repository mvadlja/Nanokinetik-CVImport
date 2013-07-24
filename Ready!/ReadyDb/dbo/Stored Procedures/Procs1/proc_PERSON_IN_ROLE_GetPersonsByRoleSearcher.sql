-- ProductsSearcher
CREATE PROCEDURE  [dbo].[proc_PERSON_IN_ROLE_GetPersonsByRoleSearcher]
	@person_role nvarchar(25) = NULL,
	@name nvarchar(25) = NULL,
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
		DistinctPERSON.* FROM
		(
			SELECT DISTINCT
			[dbo].[PERSON].person_PK AS ID, ([dbo].[PERSON].name + '' '' + [dbo].[PERSON].familyname) AS Name
			FROM [dbo].[PERSON_IN_ROLE]
			LEFT JOIN [dbo].[PERSON] ON [dbo].[PERSON].person_PK = [dbo].[PERSON_IN_ROLE].person_FK
			LEFT JOIN [dbo].[PERSON_ROLE] ON [dbo].[PERSON_ROLE].person_role_PK = [dbo].[PERSON_IN_ROLE].person_role_FK
			'
			SET @TempWhereQuery = '';

			-- Check nullability for every parameter
			-- @role_name
			IF @person_role IS NOT NULL
			BEGIN
				IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
				ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
				SET @TempWhereQuery = @TempWhereQuery + '[dbo].[PERSON_ROLE].[person_name] LIKE ''' + CONVERT (nvarchar(MAX), @person_role) + ''''
			END


			-- Check nullability for every parameter
			-- @name
			IF @name IS NOT NULL
			BEGIN
				IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
				ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
				SET @TempWhereQuery = @TempWhereQuery + '[dbo].[PERSON].[name] LIKE ''%' + REPLACE(@name,'[','[[]') + '%'''
			END

			

			IF LEN(@TempWhereQuery) > 0 BEGIN SET @Query = @Query + @TempWhereQuery; END
			SET @Query = @Query + '
		) DistinctPERSON
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SET @QueryCount = 'SELECT COUNT(DISTINCT [dbo].[PERSON_IN_ROLE].[person_in_role_PK])
					FROM [dbo].[PERSON_IN_ROLE]
					LEFT JOIN [dbo].[PERSON] ON [dbo].[PERSON].person_PK = [dbo].[PERSON_IN_ROLE].person_FK
					LEFT JOIN [dbo].[PERSON_ROLE] ON [dbo].[PERSON_ROLE].person_role_PK = [dbo].[PERSON_IN_ROLE].person_role_FK
					' + @TempWhereQuery

	EXECUTE sp_executesql @QueryCount;
END
