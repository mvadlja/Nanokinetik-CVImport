-- GetEntitiesWPS
CREATE PROCEDURE [dbo].[proc_AS2_HANDLER_LOG_GetReportDataSet]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'as2_handler_log_PK'
AS

DECLARE @Query nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		[log].[as2_handler_log_PK], [log].[log_time], [log].[received_time], [log].[as2_from],
		[log].[as2_to], [log].[message_ID], [msg].[msg_type], [log].received_message_FK,
		detailsMN.xevprm_entity_type_FK as entity_type_FK,
		CASE detailsMN.xevprm_entity_type_FK
		WHEN 1 THEN
		 (SELECT ev_code FROM AUTHORISED_PRODUCT WHERE ap_PK=detailsAP.ap_FK)
		END
		as entity_evcode,
		CASE detailsMN.xevprm_entity_type_FK
		WHEN 1 THEN
		 (SELECT product_name FROM AUTHORISED_PRODUCT WHERE ap_PK=detailsAP.ap_FK)
		END
		as entity_name, 
		detailsAP.ap_FK as entity_FK 
		FROM [dbo].[AS2_HANDLER_LOG] log
		LEFT JOIN RECIEVED_MESSAGE msg ON msg.recieved_message_PK= log.received_message_FK
		LEFT JOIN [dbo].[XEVPRM_ENTITY_DETAILS_MN] detailsMN ON detailsMN.xevprm_message_FK = msg.xevmpd_FK 
		LEFT JOIN [dbo].[XEVPRM_AP_DETAILS] detailsAP ON detailsAP.xevprm_ap_details_PK = detailsMN.xevprm_entity_details_FK
		WHERE detailsMN.xevprm_entity_type_FK = 1
		
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SELECT COUNT(*) FROM [dbo].[AS2_HANDLER_LOG]
END
