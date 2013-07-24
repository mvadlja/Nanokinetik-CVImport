-- GetAuthorisedProductsDataSet
CREATE PROCEDURE  [dbo].[proc_USER_GetUsersDataSet]
	@PageNum int = 1,
	@PageSize int = 10000,
	@OrderByQuery nvarchar(1000) = 'user_PK'
AS

DECLARE @Query nvarchar(MAX);
DECLARE @QueryCount nvarchar(MAX);
DECLARE @TempWhereQuery nvarchar(MAX);
DECLARE @Temp nvarchar(200);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		Distinct_USER.* FROM
		(
			SELECT DISTINCT
			tmpTable.user_PK,
			tmpTable.username,
			tmpTable.password,
			tmpTable.country_FK,    
			tmpTable.description,
			tmpTable.email,
			tmpTable.active,
			tmpTable.name,
			stuff ((
					SELECT cast('', '' as varchar(max)) + mainTableRoles.printName
					from (
						SELECT DISTINCT
							[dbo].[USER].user_PK,
							[dbo].[USER_ROLE].[name] as printName
						FROM [dbo].[USER]
						LEFT JOIN [dbo].[USER_IN_ROLE] ON [dbo].[USER].user_PK = [dbo].[USER_IN_ROLE].user_FK
						LEFT JOIN [dbo].[USER_ROLE] ON [dbo].[USER_IN_ROLE].user_role_FK = [dbo].[USER_ROLE].user_role_PK
					) as mainTableRoles
					WHERE mainTableRoles.user_PK = tmpTable.user_PK
					for xml path('''')
					), 1, 1, '''') AS roles
			FROM
			(
				SELECT DISTINCT
					[dbo].[USER].[user_PK], 
					[dbo].[USER].[username], 
					[dbo].[USER].[password], 
					[dbo].[USER].[country_FK], 
					[dbo].[USER].[description], 
					[dbo].[USER].[email], 
					[dbo].[USER].[active],
					(isnull([dbo].[PERSON].name,'''') +'' ''+ isnull([dbo].[PERSON].familyname,'''')) as name
				FROM [dbo].[USER]
				LEFT JOIN [dbo].[PERSON] ON [dbo].[PERSON].person_PK=[dbo].[USER].person_FK

			--ORDER BY tmpTable.user_PK 
			'
			SET @TempWhereQuery = '';

			-- Check nullability for every parameter
			IF LEN(@TempWhereQuery) > 0 BEGIN SET @Query = @Query + @TempWhereQuery; END
			SET @Query = @Query + ') as tmpTable 
		) Distinct_USER
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SET @QueryCount = '
	SELECT COUNT(DISTINCT[dbo].[USER].[user_PK])
		FROM [dbo].[USER]
		LEFT JOIN [dbo].[PERSON] ON [dbo].[PERSON].person_PK= [dbo].[USER].person_FK
	' + @TempWhereQuery

	EXECUTE sp_executesql @QueryCount;
END
