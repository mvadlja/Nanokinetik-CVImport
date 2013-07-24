-- GetEntitiesWPS
CREATE PROCEDURE  proc_XEVPRM_MESSAGE_GetEntitiesWPS
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'xevprm_message_PK'
AS

DECLARE @Query nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		[xevprm_message_PK], [message_number], [message_status_FK], [message_creation_date], [user_FK], [xml], [xml_hash], [sender_ID], [ack], [ack_type], [gateway_submission_date], [gateway_ack_date], [submitted_FK], [generated_file_name], [deleted], [received_message_FK]
		FROM [dbo].[XEVPRM_MESSAGE]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SELECT COUNT(*) FROM [dbo].[XEVPRM_MESSAGE]
END
