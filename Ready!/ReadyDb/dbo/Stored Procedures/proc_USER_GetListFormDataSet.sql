-- GetEntitiesWPS
CREATE PROCEDURE [dbo].[proc_USER_GetListFormDataSet]
	@username nvarchar(250) = NULL,
	@active nvarchar(250) = NULL,
	@name nvarchar(250) = NULL,
	@roles nvarchar(250) = NULL,

	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'user_PK'
AS

DECLARE @Query nvarchar(MAX);
DECLARE @ExecuteQuery nvarchar(MAX);
DECLARE @TempWhereQuery nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		[User].* FROM
		(
			SELECT DISTINCT
			u.[user_PK], 
			u.[username], 
			CASE u.[active]
				WHEN 1 THEN ''True''
				ELSE ''False''
			END AS active,
			(isnull(p.name,'''') +'' ''+ isnull(p.familyname,'''')) as name,
			stuff ((
					SELECT cast('', '' as varchar(max)) + UserRoleNameTable.UserRoleName
					from (
						SELECT DISTINCT
						r.[name] as UserRoleName
						FROM [dbo].[USER_IN_ROLE] urMn
						JOIN [dbo].[USER_ROLE] r ON urMn.user_role_FK = r.user_role_PK
						WHERE urMn.user_FK = u.user_PK
					) as UserRoleNameTable
					for xml path('''')
			), 1, 1, '''') AS roles
			FROM [dbo].[USER] u
			LEFT JOIN [dbo].[PERSON] p ON p.person_PK = u.person_FK
		) AS [User]
	'

	SET @TempWhereQuery = '';

		-- @username
		IF @username IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + '[User].username LIKE N''%' + REPLACE(REPLACE(@username,'[','[[]'),'''','''''') + '%'''
		END

		-- @active
		IF @active IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + '[User].active LIKE N''%' + REPLACE(REPLACE(@active,'[','[[]'),'''','''''') + '%'''
		END

		-- @name
		IF @name IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + '[User].name LIKE N''%' + REPLACE(REPLACE(@name,'[','[[]'),'''','''''') + '%'''
		END

		-- @roles
		IF @roles IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + '[User].roles LIKE N''%' + REPLACE(REPLACE(@roles,'[','[[]'),'''','''''') + '%'''
		END

		IF LEN(@TempWhereQuery) > 0 BEGIN SET @Query = @Query + @TempWhereQuery; END
		SET @Query = @Query + 
	')
	
	'
	SET @ExecuteQuery = @Query +
	'
	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @ExecuteQuery;

	SET @ExecuteQuery = @Query +
	'
	SELECT COUNT (*)
	FROM PagingResult
	'
	EXECUTE sp_executesql @ExecuteQuery;

END