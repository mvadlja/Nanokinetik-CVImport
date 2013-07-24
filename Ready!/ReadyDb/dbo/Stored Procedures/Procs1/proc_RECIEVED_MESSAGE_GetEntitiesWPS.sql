-- GetEntitiesWPS
CREATE PROCEDURE  [dbo].[proc_RECIEVED_MESSAGE_GetEntitiesWPS]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'recieved_message_PK'
AS

DECLARE @Query nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		[recieved_message_PK], [msg_data], [received_time], [processed_time], [processed], [is_successfully_processed], [msg_type], [as_header], [processing_error], [xevmpd_FK], [status]
		FROM [dbo].[RECIEVED_MESSAGE]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SELECT COUNT(*) FROM [dbo].[RECIEVED_MESSAGE]
END
