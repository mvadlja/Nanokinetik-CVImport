CREATE PROCEDURE [dbo].[proc_USER_ACTION_GetListFormDataSet]
	@display_name nvarchar(250) = NULL,
	@unique_name nvarchar(250) = NULL,
	@description nvarchar(250) = NULL,
	@Active nvarchar(250) = NULL,

	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'user_action_PK'
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
		UserAction.* FROM
		(
			SELECT DISTINCT
			ua.user_action_PK,
			ua.display_name,
			ua.unique_name,
			ua.[description],
			CASE 
				WHEN ua.active = 1 THEN ''Yes''
				ELSE ''No''
			END AS Active,
			'''' as [Delete]
			FROM dbo.USER_ACTION ua
			'
		SET @Query = @Query + 
		') AS UserAction 
		'
		SET @TempWhereQuery = '';

		-- @display_name
		IF @display_name IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'UserAction.display_name LIKE N''%' + REPLACE(REPLACE(@display_name,'[','[[]'),'''','''''') + '%'''
		END

		-- @unique_name
		IF @unique_name IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'UserAction.unique_name LIKE N''%' + REPLACE(REPLACE(@unique_name,'[','[[]'),'''','''''') + '%'''
		END

		-- @description
		IF @description IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'UserAction.[description] LIKE N''%' + REPLACE(REPLACE(@description,'[','[[]'),'''','''''') + '%'''
		END

		-- @Active
		IF @Active IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'UserAction.Active = N''' + REPLACE(REPLACE(@Active,'[','[[]'),'''','''''') + ''''
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