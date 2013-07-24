-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_SENT_MESSAGE_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [sent_message_PK]) AS RowNum,
		[sent_message_PK], [msg_data], [sent_time], [msg_type], [xevmpd_FK]
		FROM [dbo].[SENT_MESSAGE]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[SENT_MESSAGE]
END
