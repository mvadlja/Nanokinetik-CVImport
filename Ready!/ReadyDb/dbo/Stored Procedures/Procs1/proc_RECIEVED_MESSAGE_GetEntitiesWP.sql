-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_RECIEVED_MESSAGE_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [recieved_message_PK]) AS RowNum,
		[recieved_message_PK], [msg_data], [received_time], [processed_time], [processed], [is_successfully_processed], [msg_type], [as_header], [processing_error], [xevmpd_FK], [status]
		FROM [dbo].[RECIEVED_MESSAGE]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[RECIEVED_MESSAGE]
END
