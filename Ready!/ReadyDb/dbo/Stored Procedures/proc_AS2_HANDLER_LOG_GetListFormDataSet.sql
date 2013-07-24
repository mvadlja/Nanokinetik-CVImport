-- GetEntitiesWPS
CREATE PROCEDURE [dbo].[proc_AS2_HANDLER_LOG_GetListFormDataSet]
	@as2_handler_log_PK nvarchar(250) = NULL,
	@log_time nvarchar(250) = NULL,
	@received_time nvarchar(250) = NULL,
	@as2_from nvarchar(250) = NULL,
	@as2_to nvarchar(250) = NULL,
	@message_ID nvarchar(250) = NULL,
	@message_type nvarchar(250) = NULL,
	@received_message_FK nvarchar(250) = NULL,
	@entity_type nvarchar(250) = NULL,
	@entity_evcode nvarchar(250) = NULL,
	@entity_name nvarchar(250) = NULL,

	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'as2_handler_log_PK'
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
		AS2HandlerLog.* FROM
		(
			SELECT 
			[log].[as2_handler_log_PK], 
			[log].[log_time], 
			[log].[received_time], 
			[log].[as2_from],
			[log].[as2_to], 
			[log].[message_ID], 
			[msg].[msg_type], 
			[log].received_message_FK,
			detailsMN.xevprm_entity_type_FK AS entity_type_FK,

			CASE [msg].[msg_type]
				WHEN 0 THEN ''ACK''
				WHEN 1 THEN ''MDN''
				ELSE ''Unknown''
			END AS message_type,

			CASE detailsMN.xevprm_entity_type_FK
				WHEN 1 THEN (SELECT ev_code FROM AUTHORISED_PRODUCT WHERE ap_PK = detailsAP.ap_FK)
			END AS entity_evcode,

			CASE detailsMN.xevprm_entity_type_FK
				WHEN 1 THEN ''Authorised product''
			END AS entity_type,
			
			CASE detailsMN.xevprm_entity_type_FK
				WHEN 1 THEN (SELECT product_name FROM AUTHORISED_PRODUCT WHERE ap_PK = detailsAP.ap_FK)
			END AS entity_name, 

			detailsAP.ap_FK AS entity_FK 
			FROM [dbo].[AS2_HANDLER_LOG] log
			LEFT JOIN RECIEVED_MESSAGE msg ON msg.recieved_message_PK= log.received_message_FK
			LEFT JOIN [dbo].[XEVPRM_ENTITY_DETAILS_MN] detailsMN ON detailsMN.xevprm_message_FK = msg.xevmpd_FK 
			LEFT JOIN [dbo].[XEVPRM_AP_DETAILS] detailsAP ON detailsAP.xevprm_ap_details_PK = detailsMN.xevprm_entity_details_FK
			--WHERE detailsMN.xevprm_entity_type_FK = 1
		) AS AS2HandlerLog
	'

	SET @TempWhereQuery = '';

		-- @as2_handler_log_PK
		IF @as2_handler_log_PK IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AS2HandlerLog.as2_handler_log_PK LIKE N''%' + REPLACE(REPLACE(@as2_handler_log_PK,'[','[[]'),'''','''''') + '%'''
		END

		-- @log_time
		IF @log_time IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'CONVERT(NVARCHAR(30),AS2HandlerLog.log_time,4) + '' '' + CONVERT(NVARCHAR(30),AS2HandlerLog.log_time,108) LIKE ''%' + REPLACE(REPLACE(@log_time,'[','[[]'),'''','''''') + '%'''
		END

		-- @received_time
		IF @received_time IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'CONVERT(NVARCHAR(30),AS2HandlerLog.received_time,4) + '' '' + CONVERT(NVARCHAR(30),AS2HandlerLog.received_time,108) LIKE ''%' + REPLACE(REPLACE(@log_time,'[','[[]'),'''','''''') + '%'''
		END

		-- @as2_from
		IF @as2_from IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AS2HandlerLog.as2_from LIKE N''%' + REPLACE(REPLACE(@as2_from,'[','[[]'),'''','''''') + '%'''
		END

		-- @as2_to
		IF @as2_to IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AS2HandlerLog.as2_to LIKE N''%' + REPLACE(REPLACE(@as2_to,'[','[[]'),'''','''''') + '%'''
		END

		-- @message_ID
		IF @message_ID IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AS2HandlerLog.message_ID LIKE N''%' + REPLACE(REPLACE(@message_ID,'[','[[]'),'''','''''') + '%'''
		END

		-- @message_type
		IF @message_type IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AS2HandlerLog.message_type LIKE N''%' + REPLACE(REPLACE(@message_type,'[','[[]'),'''','''''') + '%'''
		END

		-- @received_message_FK
		IF @received_message_FK IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AS2HandlerLog.received_message_FK LIKE N''%' + REPLACE(REPLACE(@received_message_FK,'[','[[]'),'''','''''') + '%'''
		END

		-- @entity_type
		IF @entity_type IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AS2HandlerLog.entity_type LIKE N''%' + REPLACE(REPLACE(@entity_type,'[','[[]'),'''','''''') + '%'''
		END

		-- @entity_evcode
		IF @entity_evcode IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AS2HandlerLog.entity_evcode LIKE N''%' + REPLACE(REPLACE(@entity_evcode,'[','[[]'),'''','''''') + '%'''
		END

		-- @entity_name
		IF @entity_name IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AS2HandlerLog.entity_name LIKE N''%' + REPLACE(REPLACE(@entity_name,'[','[[]'),'''','''''') + '%'''
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