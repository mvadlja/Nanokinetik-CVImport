


CREATE PROCEDURE [dbo].[proc_RECEIVED_MESSAGE_GetListFormDataSet]
	@recieved_message_PK nvarchar(250) = NULL,
	@received_time nvarchar(250) = NULL,
	@processed_time nvarchar(250) = NULL,
	@msg_data_name nvarchar(250) = NULL,
	@is_successfully_processed_str nvarchar(250) = NULL,
	@message_type nvarchar(250) = NULL,
	@status_str nvarchar(250) = NULL,
	@processing_error nvarchar(250) = NULL,
	@xevmpd_FK nvarchar(250) = NULL,
	@entity_type nvarchar(250) = NULL,
	@entity_name nvarchar(250) = NULL,
	@entity_evcode nvarchar(250) = NULL,

	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'recieved_message_PK'
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
		ReceivedMsg.* FROM
		(
			SELECT DISTINCT
			receivedMsg.[recieved_message_PK], 
			receivedMsg.[received_time], 
			receivedMsg.[processed_time], 
			''View'' as msg_data_name,
			CASE [receivedMsg].[processed]
				WHEN 1 THEN ''Yes''
				ELSE ''No''
			END AS is_successfully_processed_str,
			CASE [receivedMsg].[msg_type]
				WHEN 0 THEN ''ACK''
				WHEN 1 THEN ''MDN''
				ELSE ''Unknown''
			END AS message_type,
			receivedMsg.[status] AS status_str,
			receivedMsg.[processing_error],
			receivedMsg.[xevmpd_FK],

			CASE detailsMN.xevprm_entity_type_FK
				WHEN 1 THEN ''Authorised product''
			END AS entity_type, 

			CASE detailsMN.xevprm_entity_type_FK
				WHEN 1 THEN (SELECT product_name FROM AUTHORISED_PRODUCT WHERE ap_PK=detailsAP.ap_FK)
			END AS entity_name,

			CASE detailsMN.xevprm_entity_type_FK
				WHEN 1 THEN (SELECT ev_code FROM AUTHORISED_PRODUCT WHERE ap_PK=detailsAP.ap_FK)
			END AS entity_evcode,

			detailsAP.ap_FK as entity_FK 

			FROM [dbo].[RECIEVED_MESSAGE] receivedMsg
			LEFT JOIN [dbo].[XEVPRM_ENTITY_DETAILS_MN] detailsMN ON detailsMN.xevprm_message_FK = receivedMsg.xevmpd_FK 
			LEFT JOIN [dbo].[XEVPRM_AP_DETAILS] detailsAP ON detailsAP.xevprm_ap_details_PK = detailsMN.xevprm_entity_details_FK
			WHERE detailsMN.xevprm_entity_type_FK = 1
			'
		SET @Query = @Query + 
		') AS ReceivedMsg 
		'
		SET @TempWhereQuery = '';

		-- @recieved_message_PK 
		IF @recieved_message_PK IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'ReceivedMsg.recieved_message_PK  LIKE N''%' + REPLACE(REPLACE(@recieved_message_PK ,'[','[[]'),'''','''''') + '%'''
		END

		-- @received_time
		IF @received_time IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'CONVERT(NVARCHAR(30),ReceivedMsg.received_time,4) + '' '' + CONVERT(NVARCHAR(30),ReceivedMsg.received_time,108) LIKE ''%' + REPLACE(REPLACE(@received_time,'[','[[]'),'''','''''') + '%'''
		END

		-- @processed_time
		IF @processed_time IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'CONVERT(NVARCHAR(30),ReceivedMsg.processed_time,4) + '' '' + CONVERT(NVARCHAR(30),ReceivedMsg.processed_time,108) LIKE ''%' + REPLACE(REPLACE(@processed_time,'[','[[]'),'''','''''') + '%'''
		END

		-- @msg_data_name 
		IF @msg_data_name IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'ReceivedMsg.msg_data_name LIKE N''%' + REPLACE(REPLACE(@msg_data_name,'[','[[]'),'''','''''') + '%'''
		END

		-- @is_successfully_processed_str 
		IF @is_successfully_processed_str IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'ReceivedMsg.is_successfully_processed_str LIKE N''%' + REPLACE(REPLACE(@is_successfully_processed_str,'[','[[]'),'''','''''') + '%'''
		END

		-- @message_type 
		IF @message_type IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'ReceivedMsg.message_type LIKE N''%' + REPLACE(REPLACE(@message_type,'[','[[]'),'''','''''') + '%'''
		END

		-- @status_str
		IF @status_str IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'ReceivedMsg.status_str LIKE N''%' + REPLACE(REPLACE(@status_str,'[','[[]'),'''','''''') + '%'''
		END

		-- @processing_error
		IF @processing_error IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'ReceivedMsg.processing_error LIKE N''%' + REPLACE(REPLACE(@processing_error,'[','[[]'),'''','''''') + '%'''
		END

		-- @xevmpd_FK
		IF @xevmpd_FK IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'ReceivedMsg.xevmpd_FK LIKE N''%' + REPLACE(REPLACE(@xevmpd_FK,'[','[[]'),'''','''''') + '%'''
		END

		-- @entity_type
		IF @entity_type IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'ReceivedMsg.entity_type LIKE N''%' + REPLACE(REPLACE(@entity_type,'[','[[]'),'''','''''') + '%'''
		END

		-- @entity_name
		IF @entity_name IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'ReceivedMsg.entity_name LIKE N''%' + REPLACE(REPLACE(@entity_name,'[','[[]'),'''','''''') + '%'''
		END

		-- @entity_evcode
		IF @entity_evcode IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'ReceivedMsg.entity_evcode LIKE N''%' + REPLACE(REPLACE(@entity_evcode,'[','[[]'),'''','''''') + '%'''
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