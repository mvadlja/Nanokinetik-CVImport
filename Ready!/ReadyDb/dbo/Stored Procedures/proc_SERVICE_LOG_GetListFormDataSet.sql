

CREATE PROCEDURE [dbo].[proc_SERVICE_LOG_GetListFormDataSet]
	@service_log_PK nvarchar(250) = NULL,
	@log_time nvarchar(250) = NULL,
	@description nvarchar(250) = NULL,
	@responsible_user nvarchar(250) = NULL,

	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'service_log_PK'
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
		ServiceLog.* FROM
		(
			SELECT DISTINCT
			log.[service_log_PK], 
			log.[description], 
			log.[log_time], 
			person.name + '' '' + person.familyName as responsible_user
			FROM [dbo].[SERVICE_LOG] log
			LEFT JOIN [dbo].[USER]  usr ON usr.user_PK = log.user_FK
			LEFT JOIN [dbo].[PERSON]  person ON person.person_PK = usr.user_PK
			'
		SET @Query = @Query + 
		') AS ServiceLog 
		'
		SET @TempWhereQuery = '';

		-- @service_log_PK
		IF @service_log_PK IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'ServiceLog.service_log_PK LIKE N''%' + REPLACE(REPLACE(@service_log_PK,'[','[[]'),'''','''''') + '%'''
		END

		-- @log_time
		IF @log_time IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'CONVERT(NVARCHAR(30),ServiceLog.log_time,4) + '' '' + CONVERT(NVARCHAR(30),ServiceLog.log_time,108) LIKE ''%' + REPLACE(REPLACE(@log_time,'[','[[]'),'''','''''') + '%'''
		END

		-- @description
		IF @description IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'ServiceLog.description LIKE N''%' + REPLACE(REPLACE(@description,'[','[[]'),'''','''''') + '%'''
		END

		-- @responsible_user
		IF @responsible_user IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'ServiceLog.responsible_user LIKE N''%' + REPLACE(REPLACE(@responsible_user,'[','[[]'),'''','''''') + '%'''
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