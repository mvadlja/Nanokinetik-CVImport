

CREATE PROCEDURE [dbo].[proc_TYPE_GetListFormDataSet]
	@TypeName nvarchar(250) = NULL,
	@GroupDescription nvarchar(250) = NULL,
	@Description nvarchar(250) = NULL,

	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'type_PK'
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
		Type.* FROM
		(
			SELECT DISTINCT
			type.type_PK,
			type.name AS TypeName,
			type.group_description AS GroupDescription,
			type.description AS Description,
			'''' as [Delete]
			FROM dbo.Type type
			'
		SET @Query = @Query + 
		') AS Type 
		'
		SET @TempWhereQuery = '';

		-- @TypeName
		IF @TypeName IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Type.TypeName LIKE N''%' + REPLACE(REPLACE(@TypeName,'[','[[]'),'''','''''') + '%'''
		END

		-- @GroupDescription
		IF @GroupDescription IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Type.GroupDescription LIKE N''%' + REPLACE(REPLACE(@GroupDescription,'[','[[]'),'''','''''') + '%'''
		END

		-- @Description
		IF @Description IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Type.Description LIKE N''%' + REPLACE(REPLACE(@Description,'[','[[]'),'''','''''') + '%'''
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