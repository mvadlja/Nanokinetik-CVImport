﻿-- GetEntitiesWPS
CREATE PROCEDURE  [dbo].[proc_SENT_MESSAGE_GetEntitiesDataSet]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'sent_message_PK'
AS

DECLARE @Query nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		[sent_message_PK], [sent_time], [msg_type], [xevmpd_FK], 
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
		FROM [dbo].[SENT_MESSAGE] sentMsg
		LEFT JOIN [dbo].[XEVPRM_ENTITY_DETAILS_MN] detailsMN ON detailsMN.xevprm_message_FK = sentMsg.xevmpd_FK 
		LEFT JOIN [dbo].[XEVPRM_AP_DETAILS] detailsAP ON detailsAP.xevprm_ap_details_PK = detailsMN.xevprm_entity_details_FK
		
		
    WHERE detailsMN.xevprm_entity_type_FK = 1
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SELECT COUNT(*) FROM [dbo].[SENT_MESSAGE]
END
