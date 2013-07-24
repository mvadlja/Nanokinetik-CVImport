

CREATE PROCEDURE [dbo].[proc_SENT_MESSAGE_GetListFormDataSet]
	@sent_message_PK nvarchar(250) = NULL,
	@sent_time nvarchar(250) = NULL,
	@xevmpd_FK nvarchar(250) = NULL,
	@message_type nvarchar(250) = NULL,
	@entity_evcode nvarchar(250) = NULL,
	@entity_name nvarchar(250) = NULL,
	@entity_type nvarchar(250) = NULL,

	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'sent_message_PK'
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
		SentMsg.* FROM
		(
			SELECT DISTINCT
			sentMsg.[sent_message_PK], 
			sentMsg.[sent_time], 
			sentMsg.[xevmpd_FK], 
			''View'' as msg_data_name,
			CASE [sentMsg].[msg_type]
				WHEN 0 THEN ''ACK''
				WHEN 1 THEN ''MDN''
				ELSE ''Unknown''
			END AS message_type,

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
			FROM [dbo].[SENT_MESSAGE] sentMsg
			LEFT JOIN [dbo].[XEVPRM_ENTITY_DETAILS_MN] detailsMN ON detailsMN.xevprm_message_FK = sentMsg.xevmpd_FK 
			LEFT JOIN [dbo].[XEVPRM_AP_DETAILS] detailsAP ON detailsAP.xevprm_ap_details_PK = detailsMN.xevprm_entity_details_FK
			WHERE detailsMN.xevprm_entity_type_FK = 1
			'
		SET @Query = @Query + 
		') AS SentMsg 
		'
		SET @TempWhereQuery = '';

		-- @sent_message_PK
		IF @sent_message_PK IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'SentMsg.xevprm_log_PK LIKE N''%' + REPLACE(REPLACE(@sent_message_PK,'[','[[]'),'''','''''') + '%'''
		END

		-- @sent_time
		IF @sent_time IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'CONVERT(NVARCHAR(30),SentMsg.sent_time,4) + '' '' + CONVERT(NVARCHAR(30),SentMsg.sent_time,108) LIKE ''%' + REPLACE(REPLACE(@sent_time,'[','[[]'),'''','''''') + '%'''
		END

		-- @xevmpd_FK
		IF @xevmpd_FK IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'SentMsg.xevmpd_FK LIKE N''%' + REPLACE(REPLACE(@xevmpd_FK,'[','[[]'),'''','''''') + '%'''
		END

		-- @message_type
		IF @message_type IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'SentMsg.message_type LIKE N''%' + REPLACE(REPLACE(@message_type,'[','[[]'),'''','''''') + '%'''
		END

		-- @entity_type
		IF @entity_type IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'SentMsg.entity_type LIKE N''%' + REPLACE(REPLACE(@entity_type,'[','[[]'),'''','''''') + '%'''
		END

		-- @entity_evcode
		IF @entity_evcode IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'SentMsg.entity_evcode LIKE N''%' + REPLACE(REPLACE(@entity_evcode,'[','[[]'),'''','''''') + '%'''
		END

		-- @entity_name
		IF @entity_name IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'SentMsg.entity_name LIKE N''%' + REPLACE(REPLACE(@entity_name,'[','[[]'),'''','''''') + '%'''
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