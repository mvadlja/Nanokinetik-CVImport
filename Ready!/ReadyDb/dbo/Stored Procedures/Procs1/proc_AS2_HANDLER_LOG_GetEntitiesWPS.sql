-- GetEntitiesWPS
CREATE PROCEDURE  [dbo].[proc_AS2_HANDLER_LOG_GetEntitiesWPS]
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
		[as2_handler_log_PK], [log_time], [event_type], [description], [received_time], [message_number], [as2_to], [as2_from], [message_ID], [filename], [received_message_FK], [connection], [date], [content_length], [content_type], [from], [host], [user_agent], [mime_version], [content_transfer_encoding], [content_disposition], [disposition_notification_to], [disposition_notification_options], [receipt_delivery_option], [ediint_features], [as2_version]
		FROM [dbo].[AS2_HANDLER_LOG]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SELECT COUNT(*) FROM [dbo].[AS2_HANDLER_LOG]
END
