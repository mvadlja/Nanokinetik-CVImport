-- GetEntitiesWP
create PROCEDURE  [dbo].[proc_MARKETING_AUTHORISATION_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [marketing_authorisation_PK]) AS RowNum,
		[marketing_authorisation_PK], [ready_id], [ma_status_FK], [message_folder], [creation_time]
		FROM [dbo].[MARKETING_AUTHORISATION]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[MARKETING_AUTHORISATION]
END
