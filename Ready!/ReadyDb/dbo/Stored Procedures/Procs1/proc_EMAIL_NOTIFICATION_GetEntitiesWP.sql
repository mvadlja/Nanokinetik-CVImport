-- GetEntitiesWP
create PROCEDURE [dbo].[proc_EMAIL_NOTIFICATION_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [email_notification_PK]) AS RowNum,
		[email_notification_PK], [notification_type_FK], [email]
		FROM [dbo].[EMAIL_NOTIFICATION]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[EMAIL_NOTIFICATION]
END
