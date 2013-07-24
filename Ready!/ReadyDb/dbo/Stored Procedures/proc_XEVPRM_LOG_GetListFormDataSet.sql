

CREATE PROCEDURE [dbo].[proc_XEVPRM_LOG_GetListFormDataSet]
	@xevprm_log_PK nvarchar(250) = NULL,
	@log_time nvarchar(250) = NULL,
	@description nvarchar(250) = NULL,
	@xevprm_message_FK nvarchar(250) = NULL,
	@entity_evcode nvarchar(250) = NULL,
	@entity_name nvarchar(250) = NULL,
	@entity_type nvarchar(250) = NULL,

	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'xevprm_log_PK'
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
		XevprmLog.* FROM
		(
			SELECT DISTINCT
			sLog.[xevprm_log_PK], 
			sLog.[log_time], 
			sLog.[description], 
			sLog.[xevprm_message_FK],
			detailsMN.xevprm_entity_type_FK as entity_type_FK,
			CASE detailsMN.xevprm_entity_type_FK
				WHEN 1 THEN (SELECT ev_code FROM AUTHORISED_PRODUCT WHERE ap_PK=detailsAP.ap_FK)
			END AS entity_evcode,
			CASE detailsMN.xevprm_entity_type_FK
				WHEN 1 THEN (SELECT product_name FROM AUTHORISED_PRODUCT WHERE ap_PK=detailsAP.ap_FK)
			END AS entity_name,
			CASE detailsMN.xevprm_entity_type_FK
				WHEN 1 THEN ''Authorised product''
			END AS entity_type,
			detailsAP.ap_FK as entity_FK 
			FROM [dbo].[XEVPRM_LOG] sLog
			LEFT JOIN [dbo].[XEVPRM_ENTITY_DETAILS_MN] detailsMN ON detailsMN.xevprm_message_FK = sLog.xevprm_message_FK 
			LEFT JOIN [dbo].[XEVPRM_AP_DETAILS] detailsAP ON detailsAP.xevprm_ap_details_PK = detailsMN.xevprm_entity_details_FK
			WHERE detailsMN.xevprm_entity_type_FK = 1
			'
		SET @Query = @Query + 
		') AS XevprmLog 
		'
		SET @TempWhereQuery = '';

		-- @xevprm_log_PK
		IF @xevprm_log_PK IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'XevprmLog.xevprm_log_PK LIKE N''%' + REPLACE(REPLACE(@xevprm_log_PK,'[','[[]'),'''','''''') + '%'''
		END

		-- @log_time
		IF @log_time IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'CONVERT(NVARCHAR(30),XevprmLog.log_time,4) + '' '' + CONVERT(NVARCHAR(30),XevprmLog.log_time,108) LIKE ''%' + REPLACE(REPLACE(@log_time,'[','[[]'),'''','''''') + '%'''
		END

		-- @description
		IF @description IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'XevprmLog.description LIKE N''%' + REPLACE(REPLACE(@description,'[','[[]'),'''','''''') + '%'''
		END

		-- @xevprm_message_FK
		IF @xevprm_message_FK IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'XevprmLog.xevprm_message_FK LIKE N''%' + REPLACE(REPLACE(@xevprm_message_FK,'[','[[]'),'''','''''') + '%'''
		END

		-- @entity_type
		IF @entity_type IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'XevprmLog.entity_type LIKE N''%' + REPLACE(REPLACE(@entity_type,'[','[[]'),'''','''''') + '%'''
		END

		-- @entity_evcode
		IF @entity_evcode IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'XevprmLog.entity_evcode LIKE N''%' + REPLACE(REPLACE(@entity_evcode,'[','[[]'),'''','''''') + '%'''
		END

		-- @entity_name
		IF @entity_name IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'XevprmLog.entity_name LIKE N''%' + REPLACE(REPLACE(@entity_name,'[','[[]'),'''','''''') + '%'''
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