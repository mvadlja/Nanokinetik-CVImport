-- GetEntitiesWP
create PROCEDURE  [dbo].[proc_MA_MESSAGE_HEADER_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [ma_message_header_PK]) AS RowNum,
		[ma_message_header_PK], [messageformatversion], [messageformatrelease], [registrationnumber], [registrationid], [readymessageid], [messagedateformat], [messagedate], [ready_id_FK], [message_file_name]
		FROM [dbo].[MA_MESSAGE_HEADER]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[MA_MESSAGE_HEADER]
END
