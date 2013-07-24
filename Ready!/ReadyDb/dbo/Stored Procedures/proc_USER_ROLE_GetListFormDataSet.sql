CREATE PROCEDURE [dbo].[proc_USER_ROLE_GetListFormDataSet]
	@display_name nvarchar(250) = NULL,
	@name nvarchar(250) = NULL,
	@description nvarchar(250) = NULL,
	@Active nvarchar(250) = NULL,
	@Actions nvarchar(250) = NULL,

	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'user_role_PK'
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
		UserRole.* FROM
		(
			SELECT DISTINCT
			ur.user_role_PK,
			ur.display_name,
			ur.name,
			ur.[description],
			CASE 
				WHEN ur.active = 1 THEN ''Yes''
				ELSE ''No''
			END AS Active,

			STUFF ( (
					SELECT CAST('', '' AS NVARCHAR(MAX)) + UserActionTable.Name
					from (
						SELECT DISTINCT
						CASE WHEN l.full_unique_path IS NOT NULL AND l.full_unique_path != '''' THEN l.full_unique_path + '' ('' + ua.display_name + '')''
						ELSE ua.display_name END AS Name
						FROM dbo.USER_ROLE_ACTION ura
						JOIN dbo.USER_ACTION ua on ua.user_action_PK = ura.user_action_FK
						JOIN dbo.LOCATION l on l.location_PK = ura.location_FK
						WHERE ura.user_role_FK = ur.user_role_PK
					) as UserActionTable
					for xml path('''') ), 1, 2, '''') AS Actions,
			'''' as [Delete]

			FROM dbo.USER_ROLE ur
			'
		SET @Query = @Query + 
		') AS UserRole 
		'
		SET @TempWhereQuery = '';

		-- @display_name
		IF @display_name IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'UserRole.display_name LIKE N''%' + REPLACE(REPLACE(@display_name,'[','[[]'),'''','''''') + '%'''
		END

		-- @name
		IF @name IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'UserRole.name LIKE N''%' + REPLACE(REPLACE(@name,'[','[[]'),'''','''''') + '%'''
		END

		-- @description
		IF @description IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'UserRole.[description] LIKE N''%' + REPLACE(REPLACE(@description,'[','[[]'),'''','''''') + '%'''
		END

		-- @Active
		IF @Active IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'UserRole.Active = N''' + REPLACE(REPLACE(@Active,'[','[[]'),'''','''''') + ''''
		END

		-- @Actions
		IF @Actions IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'UserRole.Actions LIKE N''%' + REPLACE(REPLACE(@Actions,'[','[[]'),'''','''''') + '%'''
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