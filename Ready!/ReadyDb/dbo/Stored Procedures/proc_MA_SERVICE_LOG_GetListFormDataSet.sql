


CREATE PROCEDURE [dbo].[proc_MA_SERVICE_LOG_GetListFormDataSet]
	@ma_service_log_PK nvarchar(250) = NULL,
	@event_name nvarchar(250) = NULL,
	@description nvarchar(250) = NULL,
	@ready_id_FK nvarchar(250) = NULL,

	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'ma_service_log_PK'
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
		MaServiceLog.* FROM
		(
			SELECT DISTINCT
			log.[ma_service_log_PK],
			log.[description],
			log.[ready_id_FK],
			log.[event_type_FK],
			event_type.name as event_name
			FROM [dbo].[MA_SERVICE_LOG] log
			LEFT JOIN [dbo].[MA_EVENT_TYPE] event_type ON event_type.ma_event_type_PK = log.event_type_FK
			'
		SET @Query = @Query + 
		') AS MaServiceLog 
		'
		SET @TempWhereQuery = '';

		-- @ma_service_log_PK
		IF @ma_service_log_PK IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'MaServiceLog.ma_service_log_PK LIKE N''%' + REPLACE(REPLACE(@ma_service_log_PK,'[','[[]'),'''','''''') + '%'''
		END

		-- @description
		IF @description IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'ServiceLog.description LIKE N''%' + REPLACE(REPLACE(@description,'[','[[]'),'''','''''') + '%'''
		END

		-- @ready_id_FK
		IF @ready_id_FK IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'ServiceLog.ready_id_FK LIKE N''%' + REPLACE(REPLACE(@ready_id_FK,'[','[[]'),'''','''''') + '%'''
		END

		-- @event_name
		IF @event_name IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'ServiceLog.event_name LIKE N''%' + REPLACE(REPLACE(@event_name,'[','[[]'),'''','''''') + '%'''
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